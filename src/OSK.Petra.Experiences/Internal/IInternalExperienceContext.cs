using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSK.Petra.Experiences.Internal;

internal interface IInternalExperienceContext: IExperienceContext, IDisposable
{
    void ProcessEvent(IApplicationExperienceEvent appEvent);

    void Tick(TimeSpan deltaTime);
}
