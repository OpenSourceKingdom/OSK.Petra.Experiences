namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The different statuses a challenge may possess
/// </summary>
public enum ChallengeStatus
{
    /// <summary>
    /// The challenge is active and being monitored
    /// </summary>
    Active,

    /// <summary>
    /// The challenge was completed succcessfully
    /// </summary>
    Successful,

    /// <summary>
    /// The challenge was failed
    /// </summary>
    Failed
}
