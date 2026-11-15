namespace OSK.Petra.Experiences.Models;

/// <summary>
/// Represents an objective that a user can attempt to complete for a challenge
/// </summary>
public interface IChallengeObjective
{
    /// <summary>
    /// The unique id for the objective, within the challenge
    /// </summary>
    string Id { get; }

    /// <summary>
    /// A display name for the objective
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Text insights into the objective and its purpose
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Whether the objective is considered a requirement for successfully completing a challenge
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// How the objective should be resolved within an active challenge
    /// </summary>
    ObjectiveResolutionBehavior ResolutionBehavior { get; }

    /// <summary>
    /// The initial state that the objective should be started with 
    /// </summary>
    /// <returns>The initial state for the objective</returns>
    IObjectiveData? GetInitialValue();

    /// <summary>
    /// Gets a text representation of the objective's current state
    /// </summary>
    /// <param name="state">The current state of the objective</param>
    /// <returns>A text representation of the objective's current state</returns>
    string GetValueText(ObjectiveState state);

    /// <summary>
    /// Immediately resolve the objective using the provided context and initiator to the resolution
    /// </summary>
    /// <param name="context">The context for the objective</param>
    /// <param name="initiator">The target that intiiated the resolution</param>
    void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator);
}