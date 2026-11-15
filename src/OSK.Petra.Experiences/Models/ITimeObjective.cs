using System;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Represents an objective that is updated via a time delta
/// </summary>
public interface ITimeObjective: IChallengeObjective
{
    /// <summary>
    /// Updates the objective given the elapsed delta time
    /// </summary>
    /// <param name="context">The context for the objective</param>
    /// <param name="deltaTime">The amount of time passed since the last update</param>
    void ProcessTick(IObjectiveContext context, TimeSpan deltaTime);
}
