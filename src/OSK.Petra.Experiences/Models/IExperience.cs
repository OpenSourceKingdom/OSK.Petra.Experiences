using System;

namespace OSK.Petra.Experiences.Models;

public interface IExperience
{
    Guid Id { get; }

    string Name { get; }

    string? Description { get; }

    bool IsRequired { get; }

    bool IsSecret { get; }

    ExperienceStopBehavior StopBehavior { get; }

    IExperienceEnumerator GetEnumerator();
}