using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences;

public static class ExperienceServiceExtensions
{
    extension(IExperienceService service)
    {
        public IExperienceContext? GetExperienceContext(IExperience experience)
            => service.GetExperience(experience.Id);

        public bool Remove(IExperience experience)
            => service.Remove(experience.Id);
    }
}
