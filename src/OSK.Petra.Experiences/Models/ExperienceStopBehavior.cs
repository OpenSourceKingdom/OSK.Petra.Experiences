namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Defines how an experience should be handled when a player attempts to stop it
/// </summary>
public enum ExperienceStopBehavior
{
    /// <summary>
    /// The experience will not change when attempts to stop are made
    /// </summary>
    None,

    /// <summary>
    /// The experience will be reset so that when started again it will behave as a new experience
    /// </summary>
    Reset,

    /// <summary>
    /// The experience will be paused and progression may not being tracked
    /// </summary>
    Pause,

    /// <summary>
    /// The experience will be considered ended and attempts to restart at a later time will be blocked
    /// </summary>
    End
}
