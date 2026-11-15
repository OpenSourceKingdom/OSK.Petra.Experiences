using System;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Represents an experience that a user may engage with in an application. e.g. quests, campagins, etc.
/// </summary>
public interface IExperience
{
    /// <summary>
    /// The unique id for the experience
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// A text name or title for the experience
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Insightful information for the experience purpose
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Whether the experience is required for the application
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// Whether the experience is considered a secret experience and should not be immediately shown or notified to a user
    /// </summary>
    bool IsSecret { get; }

    /// <summary>
    /// The behavior to use when a user attempts to stop an experience
    /// </summary>
    ExperienceStopBehavior StopBehavior { get; }

    /// <summary>
    /// Gets an <see cref="IExperienceEnumerator"/> that is capable of progressing the experience to completion
    /// </summary>
    /// <returns>The enumerator to progress the experience</returns>
    IExperienceEnumerator GetEnumerator();
}