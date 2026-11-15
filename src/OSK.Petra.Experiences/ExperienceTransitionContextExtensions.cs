using OSK.Petra.Experiences.Models;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences;

public static class ExperienceTransitionContextExtensions
{
    extension(IExperienceTransitionContext context)
    {
        /// <summary>
        /// Attempts to get an output from another experience
        /// </summary>
        /// <typeparam name="TOutput">The expected output type</typeparam>
        /// <param name="experience">The experience to get the output from</param>
        /// <param name="key">The key for the output</param>
        /// <param name="output">The output, if it matches the expected type and key</param>
        /// <returns>Whether the output exists as expected</returns>
        public bool TryGetOutput<TOutput>(IExperience experience, string key, [NotNullWhen(true)] out TOutput? output)
            => context.TryGetOutput(experience.Id, key, out output);
    }
}
