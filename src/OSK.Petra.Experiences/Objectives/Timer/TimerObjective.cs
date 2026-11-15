using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

/// <summary>
/// An objective that uses a timer to determine whether requirements are met
/// </summary>
/// <param name="parameters">The timer parameters used for the objectives</param>
public class TimerObjective(TimerParameters parameters) : ObjectiveBase<TimerParameters, TimerData>(parameters), ITimeObjective
{
    #region ITimeObjective

    public void ProcessTick(IObjectiveContext context, TimeSpan deltaTime)
    {
        var state = context.GetOrCreateData(_ => (TimerData)Parameters.Timer);

        var remaining = state.Value - deltaTime;
        state = remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;

        context.SetData(state);

        if (state.Value == TimeSpan.Zero)
        {
            Resolve(context, ObjectiveResolutionInitiator.Self);
            return;
        }
    }

    public override void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator)
    {
        var remaining = context.TryGetData<TimerData>(out var state)
            ? state.Value
            : Parameters.Timer;

        var expired = remaining <= TimeSpan.Zero;
        var isSatisfied = Parameters.Expectation is ObjectiveExpectation.Met ? expired : !expired;

        context.Resolve(isSatisfied);
    }

    #endregion
}
