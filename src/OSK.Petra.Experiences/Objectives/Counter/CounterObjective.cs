using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Counter;

/// <summary>
/// An objective that uses a counter to determine whether requirements are met
/// </summary>
/// <param name="parameters">The count parameters used for the objectives</param>
public class CounterObjective(CounterParameters parameters) : EventObjectiveBase<CounterParameters, CounterData, ICounterEvent>(parameters)
{
    #region EventObjectiveBase Overrides

    public override void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator)
    {
        var state = context.TryGetData<CounterData>(out var counterState)
            ? counterState.Value
            : Parameters.InitialValue;

        var reached = IsMet(state, Parameters.ThresholdValue, Parameters.Comparison);
        var satisfied = Parameters.Expectation is ObjectiveExpectation.Met ? reached : !reached;

        context.Resolve(satisfied);
    }

    protected override void ProcessEvent(IObjectiveContext context, ICounterEvent counterEvent)
    {
        if (!Parameters.Matches(counterEvent))
        {
            return;
        }

        var state = context.GetOrCreateData(_ => (CounterData)Parameters.InitialValue);
        state = state.Value + counterEvent.Delta;
        context.SetData(state);

        if (IsMet(state, Parameters.ThresholdValue, Parameters.Comparison))
        {
            Resolve(context, ObjectiveResolutionInitiator.Self);
        }
    }

    #endregion

    #region Helpers

    private bool IsMet(long value, long target, CountComparison comparison)
        => comparison switch
        {
            CountComparison.Equal => value == target,
            CountComparison.GreaterThan => value > target,
            CountComparison.GreaterThanOrEqual => value >= target,
            CountComparison.LessThan => value < target,
            CountComparison.LessThanOrEqual => value <= target,
            _ => throw new InvalidOperationException($"Comparison {comparison} is not supported")
        };

    #endregion
}
