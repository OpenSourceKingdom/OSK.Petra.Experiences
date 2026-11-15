using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// Parameters that are used and able to create <see cref="CounterObjective"/>
/// </summary>
public class CounterParameters: ObjectiveParameters<CounterParameters, CounterData>
{
    #region Static

    /// <summary>
    /// Creates a counter where reaching the threshold is success (e.g. "kill 10 wolves", "bring the boss to 0 HP").
    /// </summary>
    /// <remarks>
    /// The default comparison depends on the direction the counter moves, which is inferred from the initial value
    /// (set through <paramref id="configure"/>, default 0):
    /// <list type="bullet">
    /// <item>Counting up (initial value is at or below the threshold): <c>&gt;=</c></item>
    /// <item>Counting down (initial value is above the threshold): <c>&lt;=</c></item>
    /// </list>
    /// A comparison set explicitly in <paramref id="configure"/> overrides the default.
    /// </remarks>
    /// <param id="id">The id of the objective</param>
    /// <param id="thresholdValue">The count that, once reached, satisfies the objective</param>
    /// <param id="configure">Optional configuration for the initial value, event filters, comparison, and whether the objective is required</param>
    /// <returns>A frozen set of parameters for a counter objective that is expected to be met</returns>
    public static CounterParameters Goal(string id, long thresholdValue, Action<CounterParameters>? configure = null)
        => Create(id, thresholdValue, ObjectiveExpectation.Met, configure);

    /// <summary>
    /// Creates a counter where crossing the limit is failure (e.g. "no more than 3 deaths", "don't let fuel drop below 10").
    /// </summary>
    /// <remarks>
    /// The threshold is the value you allow, so the default comparison fails the objective only once the count passes it:
    /// <list type="bullet">
    /// <item>Counting up (initial value is at or below the threshold): <c>&gt;</c></item>
    /// <item>Counting down (initial value is above the threshold): <c>&lt;</c></item>
    /// </list>
    /// If the limit is never crossed, the challenge settles the objective as satisfied.
    /// Overriding the comparison in <paramref id="configure"/> changes what the threshold means.
    /// </remarks>
    /// <param id="id">The id of the objective</param>
    /// <param id="thresholdValue">The count the objective allows before failing</param>
    /// <param id="configure">Optional configuration for the initial value, event filters, comparison, and whether the objective is required</param>
    /// <returns>A frozen set of parameters for a counter objective that is expected to not be met</returns>
    public static CounterParameters Limit(string id, long thresholdValue, Action<CounterParameters>? configure = null)
        => Create(id, thresholdValue, ObjectiveExpectation.NotMet, configure);

    /// <summary>
    /// Builds the parameters, applies the caller's configuration, resolves the default comparison, and freezes the result.
    /// </summary>
    /// <param id="id">The id of the objective</param>
    /// <param id="thresholdValue">The count the comparison is evaluated against</param>
    /// <param id="expectation">Whether reaching the threshold is success (<see cref="ObjectiveExpectation.Met"/>) or failure (<see cref="ObjectiveExpectation.NotMet"/>)</param>
    /// <param id="configure">Optional configuration applied before defaults are resolved and the parameters are frozen</param>
    /// <returns>A frozen set of counter parameters</returns>
    private static CounterParameters Create(string id, long thresholdValue, ObjectiveExpectation expectation, Action<CounterParameters>? configure)
    {
        var parameters = new CounterParameters
        {
            Id = id,
            Expectation = expectation,
            ThresholdValue = thresholdValue
        };

        configure?.Invoke(parameters);

        // Resolved after configure so the default reflects the final InitialValue
        if (parameters._isDefaultComparison)
        {
            var countsUp = parameters.InitialValue <= thresholdValue;
            var isGoal = expectation is ObjectiveExpectation.Met;

            parameters.Comparison = (isGoal, countsUp) switch
            {
                (true, true) => CountComparison.GreaterThanOrEqual,
                (true, false) => CountComparison.LessThanOrEqual,
                (false, true) => CountComparison.GreaterThanOrEqual,
                (false, false) => CountComparison.LessThanOrEqual
            };
        }

        parameters.Validate();
        parameters.Freeze();

        return parameters;
    }

    #endregion

    #region Vaariables

    private bool _isDefaultComparison = true;

    private HashSet<string> _tags = new(StringComparer.OrdinalIgnoreCase);
    private HashSet<string> _targetIds = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The count the objective starts at.
    /// </summary>
    public long InitialValue { get; private set; }

    /// <summary>
    /// The value <see cref="Comparison"/> is evaluated against. e.g. for a goal, reaching it is success; for a limit, crossing it is failure.
    /// </summary>
    public long ThresholdValue { get; init; }

    /// <summary>
    /// Whether counter values are normalized. Set through <see cref="NormalizeValue"/>. e.g. if the goal threshold is 5 and the value goes to 7, it'll reflect 5.
    /// </summary>
    public bool Normalize { get; private set; }

    /// <summary>
    /// How the current count is compared to <see cref="ThresholdValue"/> to decide the threshold has been reached.
    /// </summary>
    public CountComparison Comparison { get; private set; }

    /// <summary>
    /// The event target ids this counter listens for. Matching is case-insensitive.
    /// Empty means the counter does not filter by target.
    /// </summary>
    public IReadOnlyCollection<string> TargetIds => _targetIds;

    /// <summary>
    /// The event tags this counter listens for. Matching is case-insensitive.
    /// Empty means the counter does not filter by tag.
    /// </summary>
    public IReadOnlyCollection<string> Tags => _tags;

    /// <summary>
    /// An optional code-only filter evaluated after the tag and target filters.
    /// Not serializable, so editor-authored content cannot depend on it. It should be pure and thread-safe.
    /// </summary>
    public Func<ICounterEvent, bool>? FilterPredicate { get; private set; }

    #endregion

    #region Api

    /// <summary>
    /// Determines whether an event should be applied to a counter using these parameters.
    /// </summary>
    /// <remarks>
    /// All configured filters must pass:
    /// <list type="bullet">
    /// <item>If tags are configured, the event must carry at least one of them.</item>
    /// <item>If target ids are configured, the event's target id must be one of them.</item>
    /// <item>If a predicate is configured, it must return true.</item>
    /// </list>
    /// With no filters configured, every event matches. An event with no tags or target id never matches a counter that filters on them.
    /// </remarks>
    /// <param id="counterEvent">The event to test</param>
    /// <returns>Whether the counter should apply the event</returns>
    /// <exception cref="ArgumentNullException">If <paramref id="counterEvent"/> is null</exception>
    public bool Matches(ICounterEvent counterEvent)
    {
        if (counterEvent is null)
        {
            throw new ArgumentNullException(nameof(counterEvent));
        }

        if (_tags.Count > 0)
        {
            if (counterEvent.Tags is not { Count: > 0 })
            {
                return false;
            }
            if (!counterEvent.Tags.Any(_tags.Contains))
            {
                return false;
            }
        }

        if (_targetIds.Count > 0)
        {
            if (string.IsNullOrWhiteSpace(counterEvent.TargetId))
            {
                return false;
            }
            if (!_targetIds.Contains(counterEvent.TargetId))
            {
                return false;
            }
        }

        return FilterPredicate is null
            ? true
            : FilterPredicate(counterEvent);
    }

    /// <summary>
    /// Sets the count the objective starts at. Use for counters that count down or that begin partway to the threshold.
    /// </summary>
    /// <param id="value">The starting count</param>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public CounterParameters WithInitialValue(long value)
    {
        ValidateFrozen();
        InitialValue = value;

        return this;
    }

    /// <summary>
    /// Adds tags the counter listens for. An event matches if it carries at least one configured tag.
    /// Can be called more than once; tags accumulate.
    /// </summary>
    /// <param id="tags">The tags to add</param>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="ArgumentNullException">If <paramref id="tags"/> is null</exception>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public CounterParameters WithTags(IEnumerable<string> tags)
    {
        ValidateFrozen();
        if (tags is null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        foreach (var tag in tags)
        {
            _tags.Add(tag);
        }

        return this;
    }

    /// <summary>
    /// Adds event target ids the counter listens for. An event matches if its target id is one of them.
    /// Can be called more than once; ids accumulate. Target ids label what happened in the game,
    /// not which objective receives the event.
    /// </summary>
    /// <param id="targetIds">The target ids to add</param>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="ArgumentNullException">If <paramref id="targetIds"/> is null</exception>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public CounterParameters WithTargets(IEnumerable<string> targetIds)
    {
        ValidateFrozen();
        if (targetIds is null)
        {
            throw new ArgumentNullException(nameof(targetIds));
        }

        foreach (var targetId in targetIds)
        {
            _targetIds.Add(targetId);
        }

        return this;
    }

    /// <summary>
    /// Overrides the default comparison. Note that this changes what <see cref="ThresholdValue"/> means,
    /// especially for limits.
    /// </summary>
    /// <remarks>
    /// <see cref="CountComparison.Equal"/> and <see cref="CountComparison.NotEqual"/> can be skipped over
    /// if an event's delta jumps past the threshold.
    /// </remarks>
    /// <param id="comparison">The comparison to use</param>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>

    public CounterParameters WithComparison(CountComparison comparison)
    {
        ValidateFrozen();
        Comparison = comparison;
        _isDefaultComparison = true;

        return this;
    }

    /// <summary>
    /// Sets a code-only filter evaluated after the tag and target filters. Replaces any previous predicate.
    /// </summary>
    /// <param id="predicate">The predicate that returns true for events the counter should apply</param>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="ArgumentNullException">If <paramref id="predicate"/> is null</exception>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public CounterParameters WithFilter(Func<ICounterEvent, bool> predicate)
    {
        ValidateFrozen();
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        FilterPredicate = predicate;

        return this;
    }

    /// <summary>
    /// Enables value normalization for the counter. See <see cref="Normalize"/>.
    /// </summary>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public CounterParameters NormalizeValue()
    {
        ValidateFrozen();

        Normalize = true;

        return this;
    }

    #endregion

    #region Helpers

    protected override IChallengeObjective CreateObjective()
        => new CounterObjective(this);

    protected override CounterData GetInitialData()
        => InitialValue;

    #endregion
}
