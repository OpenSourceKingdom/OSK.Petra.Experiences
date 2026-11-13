using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class EventObjectiveBase<TParameters, TState, TEvent>(TParameters parameters) : ObjectiveBase<TParameters, TState>(parameters), IEventObjective
    where TParameters : ObjectiveParameters<TParameters, TState>
    where TState : ICloneable
    where TEvent : IApplicationExperienceEvent
{
    #region IEventObjective

    public void ProcessEvent(IObjectiveContext context, IApplicationExperienceEvent appEvent)
    {
        if (appEvent is TEvent typedEvent)
        {
            ProcessEvent(context, typedEvent);
        }
    }

    #endregion

    #region Helpers

    protected abstract void ProcessEvent(IObjectiveContext context, TEvent appEvent);

    #endregion
}
