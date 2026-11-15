namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The expectation of an objective when it is resolved
/// </summary>
public enum ObjectiveExpectation
{
    /// <summary>
    /// The objective's requirements must be met in order to be considered satisfied (e.g. collection counter for 5 iron)
    /// </summary>
    Met,

    /// <summary>
    /// The objective's requirements must not be met in order to be considered satisfied (e.g. a loss counter for 3 lives)
    /// </summary>
    NotMet
}
