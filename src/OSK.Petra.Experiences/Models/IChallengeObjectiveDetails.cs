namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Provides details about an objective
/// </summary>
public interface IChallengeObjectiveDetails
{
    /// <summary>
    /// The objective that is being monitored
    /// </summary>
    IChallengeObjective Objective { get; }

    /// <summary>
    /// The current status for the objective
    /// </summary>
    ObjectiveStatus Status { get; }

    /// <summary>
    /// Gets a displayable text for the objective and stae
    /// </summary>
    /// <returns>The text to display</returns>
    string GetDisplayText();
}
