using OSK.Petra.Experiences.Ports;
using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// A context that provides meaningful data for <see cref="IExperienceEnumerator"/> when progressing an experience
/// </summary>
public interface IExperienceTransitionContext
{
    /// <summary>
    /// The service provider that is associated with this context
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    /// The current active challenge id. This will be null upon the first run
    /// </summary>
    string? ActiveChallengeId { get; }

    /// <summary>
    /// Attempts to get an output from the current experience
    /// </summary>
    /// <typeparam name="TOutput">The type of output expected</typeparam>
    /// <param name="key">The unique key for the output</param>
    /// <param name="output">The output</param>
    /// <returns>Whether an output matching the expectations was found</returns>
    bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output);

    /// <summary>
    /// Attempts to get an output from the another experience
    /// </summary>
    /// <typeparam name="TOutput">The type of output expected</typeparam>
    /// <param name="experienceId">The id for the experience to check</param>
    /// <param name="key">The unique key for the output</param>
    /// <param name="output">The output</param>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>Experiences referenced must be added to the <see cref="IExperienceService"/></item>
    /// </list>
    /// </remarks>
    /// <returns>Whether an output matching the expectations was found</returns>
    bool TryGetOutput<TOutput>(Guid experienceId, string key, [NotNullWhen(true)] out TOutput? output);
}
