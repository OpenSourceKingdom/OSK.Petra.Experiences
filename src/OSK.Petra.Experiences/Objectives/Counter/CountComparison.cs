namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// Describes the way that a comparison check should be applied to a <see cref="CounterObjective"/> when determining if the objective has been met
/// </summary>
public enum CountComparison
{
    /// <summary>
    /// The comparison between initial and threshold value will be equivalence
    /// </summary>
    Equal,

    /// <summary>
    /// The comparison between initial and threshold value will be greater than or equal to
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// The comparison between initial and threshold value will be greater than
    /// </summary>
    GreaterThan,

    /// <summary>
    /// The comparison between initial and threshold value will be less than or equal to
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// The comparison between initial and threshold value will be less than
    /// </summary>
    LessThan,

    /// <summary>
    /// The comparison between initial and threshold value will be not equal
    /// </summary>
    NotEqual
}