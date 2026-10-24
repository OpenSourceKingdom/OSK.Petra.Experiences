using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences;

public static class ExperienceServiceExtensions
{
    extension(IExperienceService service)
    {
        public IExperienceContext? GetContext(IExperience experience)
            => service.GetContext(experience.Id);

        public bool Remove(IExperience experience)
            => service.Remove(experience.Id);
    }
}
