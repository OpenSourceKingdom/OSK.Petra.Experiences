using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences;

public static class ExperienceServiceExtensions
{
    extension(IExperienceService service)
    {
        /// <summary>
        /// Adds an experience given an entire state
        /// </summary>
        /// <param name="experience">The experience to add</param>
        /// <param name="state">The state to use</param>
        /// <returns>A context for the experience</returns>
        public IExperienceContext AddExperience(IExperience experience, ExperienceState state)
            => service.Add(experience, new AddExperienceOptions 
            { 
                Status = state.Status,
                ChallengeStates = state.ChallengeStates.Values,
                ActiveChallengeId = state.ActiveChallengeId,
                IsPinned = state.IsPinned,
                Outputs = state.Outputs
            });

        /// <summary>
        /// Attempts to get a context for the given experience
        /// </summary>
        /// <param name="experience">The experience to get a context for</param>
        /// <returns>The context for the experience, if it was added</returns>
        public IExperienceContext? GetContext(IExperience experience)
            => service.GetContext(experience.Id);

        /// <summary>
        /// Removes the experience from the service
        /// </summary>
        /// <param name="experience">The experience to remove</param>
        /// <returns>Whether an experience was removed or not</returns>
        public bool Remove(IExperience experience)
            => service.Remove(experience.Id);
    }
}
