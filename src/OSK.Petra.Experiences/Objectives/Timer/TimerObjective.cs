using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

public class TimerObjective(TimerParameters parameters) : ObjectiveBase<TimerParameters, TimerState>(parameters), ITimeObjective
{
    #region ITimeObjective

    public void ProcessTick(IObjectiveContext context, TimeSpan deltaTime)
    {
        var state = context.GetOrCreateState(_ => (TimerState)Parameters.Timer);

        var remaining = state.Value - deltaTime;
        state = remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;

        context.SetState(state);

        if (state.Value == TimeSpan.Zero)
        {
            Resolve(context, ObjectiveResolutionInitiator.Self);
            return;
        }
    }

    public override void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator)
    {
        var remaining = context.TryGetState<TimerState>(out var state)
            ? state.Value
            : Parameters.Timer;

        var expired = remaining <= TimeSpan.Zero;
        var isSatisfied = Parameters.Expectation is ObjectiveExpectation.Met ? expired : !expired;

        context.Resolve(isSatisfied);
    }

    #endregion
}
