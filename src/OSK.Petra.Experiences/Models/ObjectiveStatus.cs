namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The statuses an objective may possess
/// </summary>
public enum ObjectiveStatus
{
    /// <summary>
    /// The objective is active and currently being monitored
    /// </summary>
    Active,

    /// <summary>
    /// The objective was finished successfully
    /// </summary>
    Successful,

    /// <summary>
    /// The objective was finished by failure
    /// </summary>
    Failed
}
