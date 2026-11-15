using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The data associated with a given challenge and its underlying objectives
/// </summary>
public class ChallengeState
{
    #region Variables

    /// <summary>
    /// The challenge id the state belongs to
    /// </summary>
    public required string ChallengeId { get; set; }

    /// <summary>
    /// The status of the challenge
    /// </summary>
    public ChallengeStatus Status { get; set; }

    /// <summary>
    /// The states for each objective the challenge possesses
    /// </summary>
    public Dictionary<string, ObjectiveState> ObjectiveStates { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    
    #endregion

    #region Api

    /// <summary>
    /// Gets the objective state via the objective id
    /// </summary>
    /// <param name="objectiveId">The id of the objective to get state for</param>
    /// <returns>The state for the objective</returns>
    /// <exception cref="KeyNotFoundException">If the key is not found</exception>
    public ObjectiveState this[string objectiveId]
        => ObjectiveStates.TryGetValue(objectiveId, out var state)
            ? state 
            : throw new KeyNotFoundException($"Objective state with id '{objectiveId}' not found.");

    /// <summary>
    /// Clones the challenge state so that data manipulations do not impact this state object
    /// </summary>
    /// <returns>A cloned state of the current data</returns>
    public ChallengeState Clone()
        => new()
        {
            ChallengeId = ChallengeId,
            Status = Status,
            ObjectiveStates = ObjectiveStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone(), StringComparer.OrdinalIgnoreCase)
        };

    #endregion
}