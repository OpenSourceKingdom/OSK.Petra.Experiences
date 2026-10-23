using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

public interface IExperienceTransitionContext
{
    IServiceProvider Services { get; }

    bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output);

    bool TryGetOutput<TOutput>(Guid experienceId, string key, [NotNullWhen(true)] out TOutput? output);
}
