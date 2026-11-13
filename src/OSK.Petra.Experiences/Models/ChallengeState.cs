using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

public class ChallengeState
{
    #region Variables

    public string ChallengeId { get; set; }

    public ChallengeStatus Status { get; set; }

    public Dictionary<string, ObjectiveState> ObjectiveStates { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    
    #endregion

    #region Api

    public ObjectiveState this[string objectiveId]
        => ObjectiveStates.TryGetValue(objectiveId, out var state)
            ? state 
            : throw new KeyNotFoundException($"Objective state with id '{objectiveId}' not found.");

    public ChallengeState Clone()
        => new()
        {
            ChallengeId = ChallengeId,
            Status = Status,
            ObjectiveStates = ObjectiveStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone(), StringComparer.OrdinalIgnoreCase)
        };

    #endregion
}