using OSK.Hexagonal.MetaData;
using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using System;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Ports;

/// <summary>
/// Performs core functions for tracking and monitoring experience states and related contexts.
/// </summary>
[HexagonalIntegration(HexagonalIntegrationType.LibraryProvided, HexagonalIntegrationType.ConsumerPointOfEntry)]
public interface IExperienceService
{
    /// <summary>
    /// Triggered when an event for an experience is fired
    /// </summary>
    event Action<IExperienceEvent>? OnExperienceEvent;

    /// <summary>
    /// Adds the given experience to the service, returning a context for managing the experience
    /// </summary>
    /// <param name="experience">The experience to add</param>
    /// <param name="options">The options for adding the experience</param>
    /// <returns>The context for managing the experience</returns>
    IExperienceContext Add(IExperience experience, AddExperienceOptions? options = null);

    /// <summary>
    /// Removes the experience and related context from the service
    /// </summary>
    /// <param name="experienceId">The id for th experience</param>
    /// <returns>Whether an experience that matched the id was removed</returns>
    bool Remove(Guid experienceId);

    /// <summary>
    /// Gets the collection of experiences that match a provided search options
    /// </summary>
    /// <param name="searchOptions">The options to filter the collection with</param>
    /// <returns>The collection of experiences that matched the given search options</returns>
    IEnumerable<IExperienceContext> GetContexts(ExperienceSearchOptions? searchOptions = null);

    /// <summary>
    /// Gets the context associated with the provided experience id
    /// </summary>
    /// <param name="experienceId">The id of the experience to get</param>
    /// <returns>The context for the experience, if an experience with the id was added</returns>
    IExperienceContext? GetContext(Guid experienceId);

    /// <summary>
    /// Updates all related contexts and experiences given a time tick
    /// </summary>
    /// <param name="deltaTime">The amount of time that has passed since the last update</param>
    void Update(TimeSpan deltaTime);

    /// <summary>
    /// Processes a given application event with all experiences added to the service
    /// </summary>
    /// <param name="appEvent">The event that was triggered by the application</param>
    void ProcessEvent(IApplicationExperienceEvent appEvent);
}
