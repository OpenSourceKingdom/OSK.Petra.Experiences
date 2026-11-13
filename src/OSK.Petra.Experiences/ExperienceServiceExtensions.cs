using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences;

public static class ExperienceServiceExtensions
{
    extension(IExperienceService service)
    {
        public IExperienceContext AddExperience(IExperience experience, ExperienceState state)
            => service.Add(experience, new AddExperienceOptions 
            { 
                Status = state.Status,
                ChallengeStates = state.ChallengeStates.Values,
                ActiveChallengeId = state.ActiveChallengeId,
                IsPinned = state.IsPinned,
                Outputs = state.Outputs
            });

        public IExperienceContext? GetContext(IExperience experience)
            => service.GetContext(experience.Id);

        public bool Remove(IExperience experience)
            => service.Remove(experience.Id);
    }
}
