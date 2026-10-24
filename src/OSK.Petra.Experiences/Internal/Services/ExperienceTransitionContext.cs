using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Ports;
using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ExperienceTransitionContext(ExperienceState state, IExperienceService service, IServiceProvider services) : IExperienceTransitionContext
{
    #region IExperienceTransitionContext

    public IServiceProvider Services => services;

    public bool TryGetOutput<TOutput>(string key, [NotNullWhen(true)] out TOutput? output)
    {
        if (state.Outputs.TryGetValue(key, out var o) && o is TOutput typedOutput)
        {
            output = typedOutput;
            return true;
        }

        output = default;
        return false;
    }

    public bool TryGetOutput<TOutput>(Guid experienceId, string key, [NotNullWhen(true)] out TOutput? output)
    {
        var experienceContext = service.GetContext(experienceId);
        if (experienceContext is not null && experienceContext.TryGetOutput(key, out output))
        {
            return true;
        }

        output = default;
        return false;
    }


    #endregion
}
