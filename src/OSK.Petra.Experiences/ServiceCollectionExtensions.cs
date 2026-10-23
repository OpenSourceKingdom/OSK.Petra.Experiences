using Microsoft.Extensions.DependencyInjection;
using OSK.Petra.Experiences.Internal;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Ports;

namespace OSK.Petra.Experiences;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddExperiences()
        {
            services.AddSingleton<IExperienceService, ExperienceService>();
            services.AddTransient<IExperienceContextProvider, ExperienceContextProvider>();

            return services;
        }
    }
}
