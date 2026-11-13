using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Internal;

internal interface IInternalExperienceContext: IExperienceContext, IDisposable
{
    event Action<IExperienceEvent>? OnEvent;

    void ProcessEvent(IApplicationExperienceEvent appEvent);

    void Tick(TimeSpan deltaTime);
}
