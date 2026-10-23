using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using System;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Ports;

public interface IExperienceService
{
    event Action<IExperienceEvent>? OnExperienceEvent;

    IExperienceContext Add(IExperience experience, AddExperienceOptions? options = null);

    bool Remove(Guid experienceId);

    IEnumerable<IExperienceContext> GetExperiences(ExperienceSearchOptions? searchOptions = null);

    IExperienceContext? GetExperience(Guid experienceId);

    void Update(TimeSpan deltaTime);

    void ProcessEvent(IApplicationExperienceEvent appEvent);
}
