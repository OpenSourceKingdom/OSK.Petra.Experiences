using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Objectives.Counter;

public class CounterParameters: ObjectiveParameters
{
    #region Static

    /// <summary>
    /// A counter where reaching the target is success (e.g. "kill 10 wolves", "bring the boss to 0 HP").
    /// Counting up defaults to >=, counting down (initialValue > targetValue) defaults to &lt;=.
    /// </summary>
    public static CounterParameters Goal(string name, long targetValue, Action<CounterParameters>? configure = null, long initialValue = 0, bool isRequired = true)
        => Goal(name, string.Empty, targetValue, configure, initialValue, isRequired);

    /// <inheritdoc cref="Goal(string, long, Action{CounterParameters}?, long, bool)"/>
    public static CounterParameters Goal(string name, string description, long targetValue, Action<CounterParameters>? configure = null, long initialValue = 0, bool isRequired = true)
        => Create(name, description, targetValue, initialValue, isRequired, ObjectiveExpectation.Met, configure);

    /// <summary>
    /// A counter where exceeding the limit is failure (e.g. "no more than 3 deaths", "don't let fuel drop below 10").
    /// The number is the value you allow: counting up defaults to > (fails once the count exceeds it),
    /// counting down defaults to &lt; (fails once the count falls below it).
    /// Overriding the comparison changes what the number means.
    /// </summary>
    public static CounterParameters Limit(string name, long targetValue, Action<CounterParameters>? configure = null, long initialValue = 0, bool isRequired = true)
        => Limit(name, string.Empty, targetValue, configure, initialValue, isRequired);

    /// <inheritdoc cref="Limit(string, long, Action{CounterParameters}?, long, bool)"/>
    public static CounterParameters Limit(string name, string description, long targetValue, Action<CounterParameters>? configure = null, long initialValue = 0, bool isRequired = true)
        => Create(name, description, targetValue, initialValue, isRequired, ObjectiveExpectation.NotMet, configure);

    private static CounterParameters Create(
        string name, string description, long targetValue, long initialValue,
        bool isRequired, ObjectiveExpectation expectation, Action<CounterParameters>? configure)
    {
        // Equal initial/target is rejected in Validate(), so the branch here only picks a default.
        var countsUp = initialValue <= targetValue;
        var isGoal = expectation is ObjectiveExpectation.Met;

        var defaultComparison = (isGoal, countsUp) switch
        {
            (true, true) => CountComparison.GreaterThanOrEqual,
            (true, false) => CountComparison.LessThanOrEqual,
            (false, true) => CountComparison.GreaterThan,
            (false, false) => CountComparison.LessThan
        };

        var parameters = new CounterParameters
        {
            Name = name,
            Description = description,
            IsRequired = isRequired,
            Expectation = expectation,
            TargetValue = targetValue,
            InitialValue = initialValue,
            Comparison = defaultComparison
        };

        configure?.Invoke(parameters);   // runs after defaults, so WithComparison wins
        parameters.Freeze();

        return parameters;
    }

    #endregion

    #region Vaariables

    private bool _frozen;

    private HashSet<string> _tags = new(StringComparer.OrdinalIgnoreCase);
    private HashSet<string> _targetIds = new(StringComparer.OrdinalIgnoreCase);

    public long InitialValue { get; init; }

    public long TargetValue { get; init; }

    public bool Normalize { get; private set; }

    public CountComparison Comparison { get; private set; }

    public IReadOnlyCollection<string> TargetIds => _targetIds;

    public IReadOnlyCollection<string> Tags => _tags;

    public Func<ICounterEvent, bool>? FilterPredicate { get; private set; }

    #endregion

    #region Api

    internal void Freeze()
    {
        _frozen = true;
    }

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

    public CounterParameters WithComparison(CountComparison comparison)
    {
        ValidateFrozen();
        Comparison = comparison;

        return this;
    }

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

    public CounterParameters NormalizeValue()
    {
        ValidateFrozen();

        Normalize = true;

        return this;
    }

    #endregion

    #region Helpers

    private void ValidateFrozen()
    {
        if (_frozen)
        {
            throw new InvalidOperationException("The parameters are frozen and can not be modified.");
        }
    }

    #endregion
}
