using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class ObjectiveParameters<TParameters>: ObjectiveParameters
    where TParameters: ObjectiveParameters<TParameters>
{
    #region Api

    /// <summary>
    /// Marks the objective as not required, so its result will not count toward the challenge outcome.
    /// </summary>
    /// <returns>The parameters, for chaining</returns>
    /// <exception cref="InvalidOperationException">If the parameters are frozen</exception>
    public TParameters NotRequired()
    {
        ValidateFrozen();
        SetNotRequired();

        return (TParameters)this;
    }

    public TParameters WithName(string name)
    {
        ValidateFrozen();
        SetName(name);
        return (TParameters)this;
    }

    public TParameters WithDescription(string description)
    {
        ValidateFrozen();
        SetDescription(description);
        return (TParameters)this;
    }

    #endregion
}
