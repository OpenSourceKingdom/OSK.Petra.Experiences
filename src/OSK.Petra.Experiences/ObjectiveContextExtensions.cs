using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences;

public static class ObjectiveContextExtensions
{
    extension(IObjectiveContext context)
    {
        /// <summary>
        /// Attempts to get the data from the context, using the factory as a fallback if not present
        /// </summary>
        /// <typeparam name="TData">The type of the data to get or create</typeparam>
        /// <param name="dataFactory">The factory to use for creating the data if it is not present</param>
        /// <returns>The data, either from the context or created by the factory</returns>
        /// <exception cref="ArgumentNullException">Thrown when the dataFactory is null</exception>
        public TData GetOrCreateData<TData>(Func<IObjectiveContext, TData> dataFactory)
            where TData : IObjectiveData
        {
            if (dataFactory is null)
            {
                throw new ArgumentNullException(nameof(dataFactory));
            }

            if (!context.TryGetData<TData>(out var data))
            {
                data = dataFactory(context);
                context.SetData(data);
            }

            return data;
        }
    }
}
