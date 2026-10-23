using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ExperienceContext(IExperience experience, ExperienceState state, IExperienceContextProvider contextProvider) : IExperienceContext, IDisposable
{
    #region Variables

    private IExperienceEnumerator _experienceEnumerator = experience.GetEnumerator(state.ChallengeStates.Max(kvp => kvp.Key));
    private ActiveExperienceChallenge? _activeExperienceChallenge;

    #endregion

    #region IExperienceContext

    public IExperience Experience => experience;

    public ExperienceStatus Status => state.Status;

    public void Start()
    {
        if (Status is not ExperienceStatus.Available)
        {
            return;
        }

        CheckExperience();
    }

    public void Stop()
    {
        if (Status is not ExperienceStatus.Active)
        {
            return;
        }
        if (!Experience.AllowStop)
        {
            return;
        }

        switch (Experience.StopBehavior)
        {
            case ExperienceStopBehavior.Pause:
                state.Status = ExperienceStatus.Paused;
                return;
            case ExperienceStopBehavior.Reset:
                state.Status = ExperienceStatus.Available;

                _experienceEnumerator.Reset();
                state.ChallengeStates.Clear();
                return;
            case ExperienceStopBehavior.End:
                state.Status = Experience.IsRequired ? ExperienceStatus.Failed : ExperienceStatus.Abandoned;
                return;
        }

        state.IsPinned = false;
        OnEvent?.Invoke(new ExperienceStatusChangeEvent(experience, state.Status));
    }

    public bool Pinned
    {
        get => state.IsPinned;
        set 
        {
            state.IsPinned = value;
            OnEvent?.Invoke(new ExperiencePinEvent(experience, state.IsPinned));
        }
    }

    public ExperienceState GetState()
        => state;

    #endregion

    #region Api

    public event Action<IExperienceEvent>? OnEvent;

    public void Dispose()
    {
        _experienceEnumerator.Dispose();
    }

    public void ProcessEvent(IApplicationExperienceEvent appEvent)
    {
        if (appEvent is null)
        {
            throw new ArgumentNullException(nameof(appEvent));
        }
        if (_activeExperienceChallenge is null)
        {
            return;
        }

        _activeExperienceChallenge.Context.Reset();
        while (_activeExperienceChallenge.Context.MoveNext() && _activeExperienceChallenge.Context.Current.Objective is IEventObjective eventObjective && _activeExperienceChallenge.Context.Current.State.Status is ObjectiveStatus.Active)
        {
            eventObjective.ProcessEvent(_activeExperienceChallenge.Context, appEvent);
            if (_activeExperienceChallenge.Context.ResolvedStatus.HasValue && _activeExperienceChallenge.Context.ResolvedStatus != _activeExperienceChallenge.Context.Current.State.Status)
            {
                _activeExperienceChallenge.Context.Current.State.Status = _activeExperienceChallenge.Context.ResolvedStatus.Value;
                OnEvent?.Invoke(new ObjectiveStatusChangeEvent(experience, _activeExperienceChallenge.Challenge, _activeExperienceChallenge.Context.Current.Objective, _activeExperienceChallenge.Context.ResolvedStatus.Value));
            }
        }

        CheckExperience();
    }

    public void Tick(TimeSpan deltaTime)
    {
        if (state.Status is not ExperienceStatus.Active)
        {
            return;
        }
        if (_activeExperienceChallenge is null)
        {
            return;
        }

        _activeExperienceChallenge.Context.Reset();
        while (_activeExperienceChallenge.Context.MoveNext() && _activeExperienceChallenge.Context.Current.Objective is ITimeObjective timeObjective && _activeExperienceChallenge.Context.Current.State.Status is ObjectiveStatus.Active)
        {
            timeObjective.ProcessTick(_activeExperienceChallenge.Context, deltaTime);
            if (_activeExperienceChallenge.Context.ResolvedStatus.HasValue && _activeExperienceChallenge.Context.ResolvedStatus != _activeExperienceChallenge.Context.Current.State.Status)
            {
                _activeExperienceChallenge.Context.Current.State.Status = _activeExperienceChallenge.Context.ResolvedStatus.Value;
                OnEvent?.Invoke(new ObjectiveStatusChangeEvent(experience, _activeExperienceChallenge.Challenge, _activeExperienceChallenge.Context.Current.Objective, _activeExperienceChallenge.Context.ResolvedStatus.Value));
            }
        }

        CheckExperience();
    }

    public bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output)
    {
        if (state.Outputs.TryGetValue(key, out var o) && o is TOutput desiredOutput)
        {
            output = desiredOutput;
            return true;
        }

        output = default;
        return false;
    }

    #endregion

    #region Helpers

    private void CheckExperience()
    {
        if (_activeExperienceChallenge is null)
        {
            MoveToNextChallenge();
            return;
        }

        var status = CheckChallenge(_activeExperienceChallenge);

        state.ChallengeStates[_activeExperienceChallenge.Challenge.Id].Status = status;

        switch (status)
        {
            case ChallengeStatus.Complete:
                OnEvent?.Invoke(new ChallengeStatusChangeEvent(experience, _activeExperienceChallenge.Challenge, ChallengeStatus.Complete));
                MoveToNextChallenge();
                break;
            case ChallengeStatus.Failed:
                state.Status = ExperienceStatus.Failed;
                OnEvent?.Invoke(new ChallengeStatusChangeEvent(experience, _activeExperienceChallenge.Challenge, ChallengeStatus.Failed));
                OnEvent?.Invoke(new ExperienceStatusChangeEvent(experience, ExperienceStatus.Failed));
                break;
        }
    }

    private ChallengeStatus CheckChallenge(ActiveExperienceChallenge activeChallenge)
    {
        var challengeStatus = EvaluateChallenge(activeChallenge.Context);
        if (challengeStatus is ChallengeStatus.Active)
        {
            return challengeStatus;
        }

        // The challenge is ending: settle everything still Active, required or optional.
        activeChallenge.Context.Reset();
        while (activeChallenge.Context.MoveNext())
        {
            if (activeChallenge.Context.Current.State.Status is not ObjectiveStatus.Active)
            {
                continue;
            }

            activeChallenge.Context.Current.Objective.Resolve(activeChallenge.Context, ObjectiveResolutionInitiator.Challenge);

            // Defensive: an objective that doesn't report is treated as passing only if the challenge is passing
            activeChallenge.Context.Current.State.Status = activeChallenge.Context.ResolvedStatus.GetValueOrDefault(challengeStatus is ChallengeStatus.Complete ? ObjectiveStatus.Complete : ObjectiveStatus.Failed);

            OnEvent?.Invoke(new ObjectiveStatusChangeEvent(experience, activeChallenge.Challenge, activeChallenge.Context.Current.Objective, activeChallenge.Context.Current.State.Status));
        }

        // A settled guard can report failure, so re-check before declaring success
        return challengeStatus is ChallengeStatus.Complete
            ? EvaluateChallenge(activeChallenge.Context)
            : challengeStatus;
    }

    private ChallengeStatus EvaluateChallenge(ObjectiveContext context)
    {
        var failedFlag = context.ChallengeFailed;   // capture in case Reset clears it
        context.Reset();

        var waiting = false;
        while (context.MoveNext())
        {
            var (objective, state) = (context.Current.Objective, context.Current.State);
            if (!objective.IsRequired)
            {
                continue;
            }

            if (state.Status is ObjectiveStatus.Failed)
            {
                return ChallengeStatus.Failed;
            }

            // Only self-resolving objectives block completion; guards are settled by the challenge
            if (state.Status is ObjectiveStatus.Active && objective.ResolutionBehavior is ObjectiveResolutionBehavior.SelfResolution)
            {
                waiting = true;
            }
        }

        return failedFlag ? ChallengeStatus.Failed
             : waiting ? ChallengeStatus.Active
             : ChallengeStatus.Complete;
    }

    private void MoveToNextChallenge()
    {
        var status = ExperienceStatus.Successful;
        if (_experienceEnumerator.MoveNext(contextProvider.GetTransitionContext(state)))
        {
            status = ExperienceStatus.Active;
            if (!state.ChallengeStates.TryGetValue(_experienceEnumerator.Current.Id, out var challengeState))
            {
                challengeState = new()
                {
                    ChallengeId = _experienceEnumerator.Current.Id,
                    Status = ChallengeStatus.Active
                };
                state.ChallengeStates[challengeState.ChallengeId] = challengeState;
            }
            var objectiveContext = contextProvider.GetObjectiveContext(state, challengeState, _experienceEnumerator.Current.GetObjectives());

            if (OnEvent is not null)
            {
                objectiveContext.Reset();
                while (objectiveContext.MoveNext())
                {
                    OnEvent.Invoke(new ObjectiveStatusChangeEvent(experience, _experienceEnumerator.Current, objectiveContext.Current.Objective, objectiveContext.Current.State.Status));
                }
            }

            _activeExperienceChallenge = new(_experienceEnumerator.Current, objectiveContext);
        }
        else
        {
            _activeExperienceChallenge = null;
        }

        if (state.Status != status)
        {
            state.Status = status;
            OnEvent?.Invoke(new ExperienceStatusChangeEvent(experience, status));
        }
    }

    #endregion
}
