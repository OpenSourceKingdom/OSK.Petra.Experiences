using System.Collections.Generic;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Provides details about a challenge, including its objectives and current status
/// </summary>
public interface IChallengeDetails
{
    /// <summary>
    /// The challenge that is being undertaken
    /// </summary>
    IExperienceChallenge Challenge { get; }

    /// <summary>
    /// The current objectives and realted insights for objectives assigned to this challenge
    /// </summary>
    IReadOnlyList<IChallengeObjectiveDetails> Objectives { get; }

    /// <summary>
    /// The current status of this challenge
    /// </summary>
    ChallengeStatus Status { get; }

    /// <summary>
    /// Gets the objective details by the objective id
    /// </summary>
    /// <param name="objectiveId">The id for the objective</param>
    /// <returns>The detail insights for the specified objective</returns>
    IChallengeObjectiveDetails this[string objectiveId] { get; }
}