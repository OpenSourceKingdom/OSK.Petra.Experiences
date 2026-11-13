using OSK.Messages.Abstractions;
using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Ports;
using System.Threading.Tasks;

namespace OSK.Extensions.Petra.Experiences.Messaging;

public class ExperienceMessageHandler(IExperienceService experienceService) : MessageRecipient<IApplicationExperienceEvent>
{
    protected override Task ReceiveAsync(MessageContext context, IApplicationExperienceEvent message)
    {
        experienceService.ProcessEvent(message);
        return Task.CompletedTask;
    }
}
