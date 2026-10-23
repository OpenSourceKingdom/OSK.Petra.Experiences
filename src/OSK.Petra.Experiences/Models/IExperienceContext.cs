using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

public interface IExperienceContext
{
    IExperience Experience { get; }

    ExperienceStatus Status { get; }

    bool Pinned { get; set; }

    void Start();

    void Stop();

    bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output);

    ExperienceState GetState();
}
