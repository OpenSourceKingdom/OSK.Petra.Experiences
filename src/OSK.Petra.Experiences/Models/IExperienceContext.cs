using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// A context that is capable of mmanaging and handling an experience and related state for a user
/// </summary>
public interface IExperienceContext
{
    /// <summary>
    /// The experience that the context references
    /// </summary>
    IExperience Experience { get; }

    /// <summary>
    /// The active challenge for the experience, if it is available
    /// </summary>
    IChallengeDetails? ActiveChallenge { get; }

    /// <summary>
    /// The current status for the experience
    /// </summary>
    ExperienceStatus Status { get; }

    /// <summary>
    /// Whether the user has pinned this experience
    /// </summary>
    bool Pinned { get; set; }

    /// <summary>
    /// Attempts to start the experience
    /// </summary>
    void Start();

    /// <summary>
    /// Attemps to stop the experience
    /// </summary>
    void Stop();

    /// <summary>
    /// Attempts to get a named key output for the experience
    /// </summary>
    /// <typeparam name="TOutput">The type of output expected for the key</typeparam>
    /// <param name="key">The unique key for the output</param>
    /// <param name="output">The output</param>
    /// <returns>Whether an output that was expected exists</returns>
    bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output);

    /// <summary>
    /// Gets a snapshot of the current state within the experience context, including challenge states and outputs
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>The state snapshot is a point-in-time representation of the experience context and is not updated in real-time.</item>
    /// </list>
    /// </remarks>
    /// <returns>The experience state snapshot</returns>
    ExperienceState GetStateSnapshot();
}
