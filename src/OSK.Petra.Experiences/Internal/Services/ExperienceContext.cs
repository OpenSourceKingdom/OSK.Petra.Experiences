using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ExperienceContext(IExperience experience, ExperienceState state, IExperienceContextProvider contextProvider) : IInternalExperienceContext
{
    #region Variables

    private IExperienceEnumerator _experienceEnumerator = experience.GetEnumerator();

    private ActiveExperienceChallenge? _activeChallenge;

    #endregion

    #region IExperienceContext

    public event Action<IExperienceEvent>? OnEvent;

    public IExperience Experience => experience;

    public ExperienceStatus Status => state.Status;

    public IChallengeDetails? ActiveChallenge => _activeChallenge;

    public void Start()
    {
        switch (Status)
        {
            case ExperienceStatus.Paused:
                state.Status = ExperienceStatus.Active;
                OnEvent?.Invoke(new ExperienceStatusChangeEvent(this, ExperienceStatus.Active));
                break;
            case ExperienceStatus.Available:
                CheckExperience();
                break;
            case ExperienceStatus.Active:
                if (ActiveChallenge is null)
                {
                    CheckExperience();
                }
                break;
        }
    }

    public void Stop()
    {
        if (Status is not ExperienceStatus.Active)
        {
            return;
        }
        if (Experience.StopBehavior is ExperienceStopBehavior.None)
        {
            return;
        }

        switch (Experience.StopBehavior)
        {
            case ExperienceStopBehavior.Pause:
                state.Status = ExperienceStatus.Paused;
                break;
            case ExperienceStopBehavior.Reset:
                state.Status = ExperienceStatus.Available;

                _experienceEnumerator.Reset();
                state.ChallengeStates.Clear();
                break;
            case ExperienceStopBehavior.End:
                state.Status = Experience.IsRequired ? ExperienceStatus.Failed : ExperienceStatus.Abandoned;
                break;
        }

        state.IsPinned = false;
        OnEvent?.Invoke(new ExperienceStatusChangeEvent(this, state.Status));
    }

    public bool Pinned
    {
        get => state.IsPinned;
        set 
        {
            if (state.IsPinned == value)
            {
                return;
            }

            state.IsPinned = value;
            OnEvent?.Invoke(new ExperiencePinEvent(this, state.IsPinned));
        }
    }

    public ExperienceState GetState()
        => state.Clone();

    #endregion

    #region Api

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
        if (_activeChallenge is null)
        {
            return;
        }

        _activeChallenge.ForEach((context, details) =>
        {
            if (details.Status is not ObjectiveStatus.Active || details.Objective is not IEventObjective eventObjective)
            {
                return;
            }

            eventObjective.ProcessEvent(context, appEvent);

            if (context.UpdatedState)
            {
                OnEvent?.Invoke(new ObjectiveUpdatedEvent(this, _activeChallenge.Challenge, details.Objective));
            }

            if (context.ResolvedStatus.HasValue && context.ResolvedStatus != details.Status)
            {
                details.Status = context.ResolvedStatus.Value;
                OnEvent?.Invoke(new ObjectiveStatusChangeEvent(this, _activeChallenge.Challenge, details.Objective, context.ResolvedStatus.Value));
            }
        });

        CheckExperience();
    }

    public void Tick(TimeSpan deltaTime)
    {
        if (state.Status is not ExperienceStatus.Active)
        {
            return;
        }
        if (_activeChallenge is null)
        {
            return;
        }

        _activeChallenge.ForEach((context, details) =>
        {
            if (details.Status is not ObjectiveStatus.Active || details.Objective is not ITimeObjective timeObjective)
            {
                return;
            }

            timeObjective.ProcessTick(context, deltaTime);

            if (context.UpdatedState)
            {
                OnEvent?.Invoke(new ObjectiveUpdatedEvent(this, _activeChallenge.Challenge, details.Objective));
            }
            if (context.ResolvedStatus.HasValue && context.ResolvedStatus != details.Status)
            {
                details.Status = context.ResolvedStatus.Value;
                OnEvent?.Invoke(new ObjectiveStatusChangeEvent(this, _activeChallenge.Challenge, details.Objective, context.ResolvedStatus.Value));
            }
        });

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
        if (_activeChallenge is null)
        {
            MoveToNextChallenge();
            return;
        }

        var status = ProcessChallenge(_activeChallenge);

        state.ChallengeStates[_activeChallenge.Challenge.Id].Status = status;

        switch (status)
        {
            case ChallengeStatus.Complete:
                OnEvent?.Invoke(new ChallengeStatusChangeEvent(this, _activeChallenge.Challenge, ChallengeStatus.Complete));
                MoveToNextChallenge();
                break;
            case ChallengeStatus.Failed:
                state.Status = ExperienceStatus.Failed;
                OnEvent?.Invoke(new ChallengeStatusChangeEvent(this, _activeChallenge.Challenge, ChallengeStatus.Failed));
                OnEvent?.Invoke(new ExperienceStatusChangeEvent(this, ExperienceStatus.Failed));
                break;
        }
    }

    private ChallengeStatus ProcessChallenge(ActiveExperienceChallenge activeChallenge)
    {
        var challengeStatus = EvaluateChallenge(activeChallenge);
        if (challengeStatus is ChallengeStatus.Active)
        {
            return challengeStatus;
        }

        // The challenge is ending: settle everything still Active, required or optional
        activeChallenge.ForEach((context, details) =>
        {
            if (details.Status is not ObjectiveStatus.Active)
            {
                return;
            }

            details.Objective.Resolve(context, ObjectiveResolutionInitiator.Challenge);

            // Defensive: an details that doesn't report is treated as passing only if the challenge is passing
            details.Status = context.ResolvedStatus.GetValueOrDefault(challengeStatus is ChallengeStatus.Complete ? ObjectiveStatus.Complete : ObjectiveStatus.Failed);

            if (context.UpdatedState)
            {
                OnEvent?.Invoke(new ObjectiveUpdatedEvent(this, activeChallenge.Challenge, details.Objective));
            }
            OnEvent?.Invoke(new ObjectiveStatusChangeEvent(this, activeChallenge.Challenge, details.Objective, details.Status));
        });

        
        // A settled guard can report failure, so re-check before declaring success
        return challengeStatus is ChallengeStatus.Complete
            ? EvaluateChallenge(activeChallenge)
            : challengeStatus;
    }

    private ChallengeStatus EvaluateChallenge(ActiveExperienceChallenge challenge)
    {
        var failedFlag = challenge.ChallengeFailed;
        var hasActiveObjectives = false;

        if (!failedFlag)
        {
            challenge.ForEach((context, details) =>
            {
                if (!details.Objective.IsRequired || failedFlag)
                {
                    return;
                }

                switch (details.Status)
                {
                    case ObjectiveStatus.Failed:
                        failedFlag = true;
                        break;
                    // Only self-resolving objectiveDetails block completion; guards are settled by the challenge
                    case ObjectiveStatus.Active when details.Objective.ResolutionBehavior is ObjectiveResolutionBehavior.SelfResolution:
                        hasActiveObjectives = true;
                        break;
                }
            }, allowBreakOnFail: true);
        }

        return failedFlag 
                ? ChallengeStatus.Failed
                : hasActiveObjectives ? ChallengeStatus.Active : ChallengeStatus.Complete;
    }

    private void MoveToNextChallenge()
    {
        var status = ExperienceStatus.Complete;
        var transitionContext = contextProvider.GetTransitionContext(state);
        if (_experienceEnumerator.MoveNext(transitionContext))
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

            state.ActiveChallengeId = challengeState.ChallengeId;

            var objectiveContext = contextProvider.GetObjectiveContext(state, challengeState);
            var objectiveDetails = _experienceEnumerator.Current.GetObjectives(transitionContext).Select(objective =>
            {
                if (!challengeState.ObjectiveStates.TryGetValue(objective.Id, out var state))
                {
                    state = new()
                    {
                        ObjectiveId = objective.Id,
                        Status = ObjectiveStatus.Active
                    };
                    challengeState.ObjectiveStates[objective.Id] = state;
                }

                state.Data = objective.GetInitialValue();
                return new ObjectiveDetails(objective, state);
            }).ToArray();

            _activeChallenge = new(_experienceEnumerator.Current, objectiveContext, objectiveDetails);

            if (OnEvent is not null)
            {
                OnEvent.Invoke(new ChallengeStatusChangeEvent(this, _experienceEnumerator.Current, ChallengeStatus.Active));
                _activeChallenge.ForEach((_, details) =>
                {
                    OnEvent.Invoke(new ObjectiveAddedEvent(this, _experienceEnumerator.Current, details.Objective));
                });
            }
        }
        else
        {
            _activeChallenge = null;
        }

        if (state.Status != status)
        {
            state.Status = status;
            OnEvent?.Invoke(new ExperienceStatusChangeEvent(this, status));
        }
    }

    #endregion
}
