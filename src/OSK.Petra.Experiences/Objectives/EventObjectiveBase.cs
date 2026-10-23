using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Objectives;

public abstract class EventObjectiveBase<TParameters, TEvent>(TParameters parameters) : ObjectiveBase<TParameters>(parameters), IEventObjective
    where TParameters : ObjectiveParameters
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
