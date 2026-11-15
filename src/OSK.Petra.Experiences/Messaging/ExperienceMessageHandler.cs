using OSK.Messages.Abstractions;
using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Ports;
using System.Threading.Tasks;

namespace OSK.Petra.Experiences.Messaging;

/// <summary>
/// A message recipient that processes application events into the experience runtime
/// </summary>
/// <param name="experienceService">The service to handle app events</param>
public class ExperienceMessageHandler(IExperienceService experienceService) : MessageRecipient<IApplicationExperienceEvent>
{
    protected override Task ReceiveAsync(MessageContext context, IApplicationExperienceEvent message)
    {
        experienceService.ProcessEvent(message);
        return Task.CompletedTask;
    }
}
