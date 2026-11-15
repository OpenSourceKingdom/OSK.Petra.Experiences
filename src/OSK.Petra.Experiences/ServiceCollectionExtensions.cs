using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSK.Messages.Abstractions;
using OSK.Petra.Experiences.Internal;
using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Ports;
using OSK.Petra.Experiences.Messaging;

namespace OSK.Petra.Experiences;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds experience services and dependencies to the service collection
        /// </summary>
        /// <returns>The services for chaining</returns>
        public IServiceCollection AddExperiences()
        {
            services.TryAddSingleton<IExperienceService, ExperienceService>();
            services.TryAddTransient<IExperienceContextProvider, ExperienceContextProvider>();

            services.TryAddMessageRecipient<ExperienceMessageHandler>();

            return services;
        }
    }
}
