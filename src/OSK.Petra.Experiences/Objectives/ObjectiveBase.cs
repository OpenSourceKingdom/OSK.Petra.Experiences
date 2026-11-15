using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

/// <summary>
/// A base class for initial logic for challenge objectives that utilize a set of <see cref="ObjectiveParameters"/>
/// </summary>
/// <typeparam name="TParameters">The type of parameters the obejctive uses</typeparam>
public abstract class ObjectiveBase<TParameters>: IChallengeObjective
    where TParameters: ObjectiveParameters
{
    #region Variables

    protected TParameters Parameters { get; }

    #endregion

    #region Constructors

    protected ObjectiveBase(TParameters parameters)
    {
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        Parameters.Validate();
    }

    #endregion

    #region IChallengeObjective

    public string Id => Parameters.Id;

    public string Name => Parameters.Name;

    public string? Description => Parameters.Description;

    public bool IsRequired => Parameters.IsRequired;

    public ObjectiveResolutionBehavior ResolutionBehavior => Parameters.GetEffectiveResolutionBehavior();

    public IObjectiveData? GetInitialValue()
        => Parameters.GetInitialValue();

    public abstract string GetValueText(ObjectiveState state);

    public abstract void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator);


    #endregion
}
