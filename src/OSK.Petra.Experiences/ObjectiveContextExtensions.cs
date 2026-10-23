using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences;

public static class ObjectiveContextExtensions
{
    extension(IObjectiveContext context)
    {
        public TState GetOrCreateState<TState>(Func<IObjectiveContext, TState> stateFactory)
            where TState: ICloneable
        {
            if (stateFactory is null)
            {
                throw new ArgumentNullException(nameof(stateFactory));
            }

            if (!context.TryGetState<TState>(out var state))
            {
                state = stateFactory(context);
                context.SetState(state);
            }

            return state;
        }
    }
}
