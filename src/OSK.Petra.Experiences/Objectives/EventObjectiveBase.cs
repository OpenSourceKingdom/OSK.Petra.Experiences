using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Objectives;

/// <summary>
/// A base class for objectives that use events to process requirements
/// </summary>
/// <typeparam name="TParameters">The type of parameters for the objective</typeparam>
/// <typeparam name="TData">The type of data that is associated with the objective</typeparam>
/// <typeparam name="TEvent">The type of event that is processed by the objective</typeparam>
/// <param name="parameters">The parameters that are assigned to the objective</param>
public abstract class EventObjectiveBase<TParameters, TData, TEvent>(TParameters parameters) : ObjectiveBase<TParameters, TData>(parameters), IEventObjective
    where TParameters : ObjectiveParameters<TParameters, TData>
    where TData : IObjectiveData
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

    /// <summary>
    /// Process the typed event against the objective
    /// </summary>
    /// <param name="context">The context that the objective references for processing</param>
    /// <param name="appEvent">The typed event</param>
    protected abstract void ProcessEvent(IObjectiveContext context, TEvent appEvent);

    #endregion
}
