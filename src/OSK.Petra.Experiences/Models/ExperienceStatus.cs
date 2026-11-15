namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The different statuses an experience can possess
/// </summary>
public enum ExperienceStatus
{
    /// <summary>
    /// The experience is not available for a user and can not be started
    /// </summary>
    Unavailable,

    /// <summary>
    /// The experience is available for a user and can be started whenever they choose
    /// </summary>
    Available,

    /// <summary>
    /// The experience was previously started by the player, but is currently being paused
    /// </summary>
    Paused,

    /// <summary>
    /// The experience is currently being engaged by the user
    /// </summary>
    Active,

    /// <summary>
    /// The experience has been successfully completed
    /// </summary>
    Successful,

    /// <summary>
    /// The experience has failed
    /// </summary>
    Failed,

    /// <summary>
    /// The experience has been abandoned
    /// </summary>
    Abandoned
}
