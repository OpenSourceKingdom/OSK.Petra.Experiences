using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

public interface IObjectiveContext
{
    IServiceProvider Services { get; }

    void Resolve(bool satisfied);

    bool TryGetState<TState>([NotNullWhen(true)] out TState? state);

    void SetState<TState>(TState state)
        where TState : ICloneable;

    void SetOutput<TOutput>(string key, TOutput output)
        where TOutput : ICloneable;
}
