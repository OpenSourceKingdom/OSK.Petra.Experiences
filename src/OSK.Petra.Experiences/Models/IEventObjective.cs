using OSK.Petra.Experiences.Events;

namespace OSK.Petra.Experiences.Models;

public interface IEventObjective
{
    void ProcessEvent(IObjectiveContext context, IApplicationExperienceEvent appEvent);
}
