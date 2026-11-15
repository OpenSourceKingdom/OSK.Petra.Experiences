using System.Collections.Generic;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Represents a challenge that can be undertaken by a user within an experience
/// </summary>
public interface IExperienceChallenge
{
    /// <summary>
    /// A unique id for the challenge within the experience
    /// </summary>
    string Id { get; }

    /// <summary>
    /// A text displayable name or title
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Insightful infromation for the purpose of the challenge
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Gets the collection of objectives associated with this challenge
    /// </summary>
    /// <param name="context">The context that is used to determine the objectives that the challenge will use</param>
    /// <returns>The collection of objectives the challenge possesses</returns>
    IReadOnlyList<IChallengeObjective> GetObjectives(IExperienceTransitionContext context);
}
