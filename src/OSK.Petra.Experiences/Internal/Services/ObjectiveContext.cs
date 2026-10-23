using OSK.Petra.Experiences.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ObjectiveContext : IObjectiveContext, IEnumerator<(IChallengeObjective Objective, ObjectiveState State)>
{
    #region Variables

    public bool ChallengeFailed { get; private set; }

    public ObjectiveStatus? ResolvedStatus { get; private set; }

    private readonly ExperienceState _experienceState;
    private readonly ChallengeState _challengeState;
    private readonly IReadOnlyList<(IChallengeObjective, ObjectiveState)> _objectiveStatePairs;

    private int _objectiveId = -1;

    #endregion

    #region Constructors

    public ObjectiveContext(ExperienceState experienceState, ChallengeState challengeState, IReadOnlyList<IChallengeObjective> objectives, IServiceProvider services)
    {
        _experienceState = experienceState ?? throw new ArgumentNullException(nameof(experienceState));
        _challengeState = challengeState ?? throw new ArgumentNullException(nameof(challengeState));
        Services = services ?? throw new ArgumentNullException(nameof(services));

        _objectiveStatePairs = objectives?.Select((objective, index) =>
        {
            if (!challengeState.ObjectiveStates.TryGetValue(index, out var state))
            {
                state = new()
                {
                    Status = ObjectiveStatus.Active
                };
                challengeState.ObjectiveStates[index] = state;
            }

            return (objective, state);
        }).ToArray() ?? throw new ArgumentNullException(nameof(objectives));
    }

    #endregion

    #region IEnumerator

    public (IChallengeObjective Objective, ObjectiveState State) Current => _objectiveStatePairs[_objectiveId];

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_objectiveId >= _objectiveStatePairs.Count)
        {
            return false;
        }

        _objectiveId++;
        return true;
    }

    public void Reset()
    {
        _objectiveId = -1;
    }

    public void Dispose()
    {
    }

    #endregion

    #region IObjectiveContext

    public IServiceProvider Services { get; }

    public void Resolve(bool satisfied)
    {
        ResolvedStatus = satisfied
            ? ObjectiveStatus.Complete
            : ObjectiveStatus.Failed;

        ChallengeFailed = Current.Objective.IsRequired && !satisfied;
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
        if (_challengeState.ObjectiveStates.TryGetValue(_objectiveId, out var objectiveState))
        {
            objectiveState.Data = state;
            return;
        }

        _challengeState.ObjectiveStates[_objectiveId] = new()
        {
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
