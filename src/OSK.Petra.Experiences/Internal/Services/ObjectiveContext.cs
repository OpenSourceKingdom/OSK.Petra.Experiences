using OSK.Petra.Experiences.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ObjectiveContext : IObjectiveContext
{
    #region Variables

    public ObjectiveStatus? ResolvedStatus { get; private set; }
    public bool UpdatedState { get; private set; }

    private readonly ExperienceState _experienceState;
    private readonly ChallengeState _challengeState;

    private string _objectiveId;

    #endregion

    #region Constructors

    public ObjectiveContext(ExperienceState experienceState, ChallengeState challengeState, IServiceProvider services)
    {
        _experienceState = experienceState ?? throw new ArgumentNullException(nameof(experienceState));
        _challengeState = challengeState ?? throw new ArgumentNullException(nameof(challengeState));
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    #endregion

    #region Api

    public ChallengeState ChallengeState => _challengeState;

    public void SetObjective(string id)
    {
        _objectiveId = id;
        ResolvedStatus = null;
        UpdatedState = false;
    }

    #endregion

    #region IObjectiveContext

    public IServiceProvider Services { get; }

    public void Resolve(bool satisfied)
    {
        ResolvedStatus = satisfied
            ? ObjectiveStatus.Complete
            : ObjectiveStatus.Failed;
    }

    public bool TryGetState<TState>([NotNullWhen(true)]out TState? state)
    {
        if (_challengeState.ObjectiveStates.TryGetValue(_objectiveId, out var objectiveState) && objectiveState is { Data: TState typedState })
        {
            state = typedState;
            return true;
        }

        state = default;
        return false;
    }

    public void SetState<TState>(TState state)
        where TState: ICloneable
    {
        UpdatedState = true;
        if (_challengeState.ObjectiveStates.TryGetValue(_objectiveId, out var objectiveState))
        {
            objectiveState.Data = state;
            return;
        }

        _challengeState.ObjectiveStates[_objectiveId] = new()
        {
            ObjectiveId = _objectiveId,
            Status = ObjectiveStatus.Active,
            Data = state
        };
    }

    public void SetOutput<TOutput>(string key, TOutput output)
        where TOutput: ICloneable
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(nameof(key));
        }

        _experienceState.Outputs[key] = output;
    }

    #endregion
}
