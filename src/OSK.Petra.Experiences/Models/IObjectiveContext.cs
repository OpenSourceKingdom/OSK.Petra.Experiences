using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// A context that provides functions for an objective processing an update or resolving
/// </summary>
public interface IObjectiveContext
{
    /// <summary>
    /// The services associated with this context
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    /// Resolves the current objective as failed or succesful
    /// </summary>
    /// <param name="satisfied">Whether the objective's requirements were met</param>
    void Resolve(bool satisfied);

    /// <summary>
    /// Attempts to get the data for the objective
    /// </summary>
    /// <typeparam name="TData">The expected type of data</typeparam>
    /// <param name="data">The stored data</param>
    /// <returns>Whether the data existed</returns>
    bool TryGetData<TData>([NotNullWhen(true)] out TData? data);

    /// <summary>
    /// Sets the objective data for storage
    /// </summary>
    /// <typeparam name="TData">The type of data</typeparam>
    /// <param name="data">The data to store</param>
    void SetData<TData>(TData data)
        where TData : IObjectiveData;

    /// <summary>
    /// Creates an easily accessible output for the experience that can be referenced via <see cref="IExperienceTransitionContext"/> when progressing experiences.
    /// </summary>
    /// <typeparam name="TOutput">The type of output to store</typeparam>
    /// <param name="key">The unique key for the output</param>
    /// <param name="output">The output to store</param>
    void SetOutput<TOutput>(string key, TOutput output)
        where TOutput : ICloneable;
}
