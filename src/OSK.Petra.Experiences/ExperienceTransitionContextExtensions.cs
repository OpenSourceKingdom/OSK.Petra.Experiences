using OSK.Petra.Experiences.Models;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences;

public static class ExperienceTransitionContextExtensions
{
    extension(IExperienceTransitionContext context)
    {
        public bool TryGetOutput<TOutput>(IExperience experience, string key, [NotNullWhen(true)] out TOutput? output)
            => context.TryGetOutput(experience.Id, key, out output);
    }
}
