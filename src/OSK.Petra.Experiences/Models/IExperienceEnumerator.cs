using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// An enumerator that is capable of progressing an experience when moved
/// </summary>
public interface IExperienceEnumerator: IDisposable
{
    /// <summary>
    /// The current challenge, if one is available
    /// </summary>
    IExperienceChallenge? Current { get; }

    /// <summary>
    /// Attempts to progress the experience
    /// </summary>
    /// <param name="context">A context object to reference when progressing the experience</param>
    /// <returns>Whether the experience was successfully progressed and a new challenge is available</returns>
    [MemberNotNullWhen(true, nameof(Current))]
    bool MoveNext(IExperienceTransitionContext context);

    /// <summary>
    /// Resets the enumerator to start the experience from the start
    /// </summary>
    void Reset();
}
