using OSK.Petra.Experiences.Events;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Objectives.Counter;

public interface ICounterEvent: IApplicationExperienceEvent
{
    long Delta { get; }

    string? TargetId { get; }

    IReadOnlyCollection<string> Tags { get; }
}
