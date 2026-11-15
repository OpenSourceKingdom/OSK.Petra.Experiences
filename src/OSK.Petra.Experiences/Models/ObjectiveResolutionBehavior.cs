namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The behavior an objective's internal logic takes to finish being processed
/// </summary>
public enum ObjectiveResolutionBehavior
{
    /// <summary>
    /// Objective is expected to be completed and resolved only when the objective is satisfied on its own
    /// </summary>
    SelfResolution,

    /// <summary>
    /// The objective is expected to be completed once the challenge resolves
    /// </summary>
    ChallengeResolution
}
