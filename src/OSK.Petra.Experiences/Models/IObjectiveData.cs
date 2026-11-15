using System;

namespace OSK.Petra.Experiences.Models;

/// <summary>
/// The data specific to a particular objective
/// </summary>
public interface IObjectiveData: ICloneable
{
    /// <summary>
    /// The raw data value for the data
    /// </summary>
    object Value { get; }
}
