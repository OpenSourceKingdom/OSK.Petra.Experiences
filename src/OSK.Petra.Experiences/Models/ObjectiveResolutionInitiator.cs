namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The initiator of an objective's resolution
/// </summary>
public enum ObjectiveResolutionInitiator
{
    /// <summary>
    /// The objective is being resolved because internal requirements caused it to be resolved
    /// </summary>
    Self,

    /// <summary>
    /// The objective is being resolved because the challenge is finishing processing
    /// </summary>
    Challenge
}
