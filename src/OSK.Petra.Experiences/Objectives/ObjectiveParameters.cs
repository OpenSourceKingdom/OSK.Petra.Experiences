using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

/// <summary>
/// A base class for objective parameters
/// </summary>
public abstract class ObjectiveParameters
{
    #region Variables

    private string? _name;

    private bool _frozen;

    private bool _isRequired = true;

    /// <summary>
    /// A unique id for the objective
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// A name or title for the objective
    /// </summary>
    public string Name 
    { 
        get => string.IsNullOrWhiteSpace(_name) ? Id : _name;
        private set => _name = value;
    }

    /// <summary>
    /// Insightful text that describes the purpose of the objective
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Whether the objective is required
    /// </summary>
    public bool IsRequired 
    {
        get => _isRequired;
        init
        {
            _isRequired = value;
        } 
    } 

    /// <summary>
    /// The expectation of the objective when it is resolved
    /// </summary>
    public ObjectiveExpectation Expectation { get; init; } = ObjectiveExpectation.Met;

    /// <summary>
    /// The style of resolution for the objective
    /// </summary>
    public ObjectiveResolutionBehavior? ResolutionBehavior { get; init; }

    #endregion

    #region Api

    /// <summary>
    /// Gets the initial data value for the objective
    /// </summary>
    /// <returns>The initial objective data</returns>
    public abstract IObjectiveData? GetInitialValue();

    /// <summary>
    /// Converts the parameters to an objective
    /// </summary>
    /// <returns>The objective the parameters refer to</returns>
    public IChallengeObjective ToObjective()
    {
        ValidateParameters();
        return CreateObjective();
    }

    /// <summary>
    /// The effective resolution behavior for the objective
    /// </summary>
    /// <returns>The actual effective resolution behavior</returns>
    public ObjectiveResolutionBehavior GetEffectiveResolutionBehavior()
        => ResolutionBehavior
           ?? (Expectation is ObjectiveExpectation.Met
                ? ObjectiveResolutionBehavior.SelfResolution
                : ObjectiveResolutionBehavior.ChallengeResolution);

    /// <summary>
    /// Validates whether the parameter settings. This is expected to throw if settings are invalid.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the id is null or empty</exception>
    /// <exception cref="InvalidOperationException">If the objective expectation and resolution behavior cause an invalid parameter state</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            throw new ArgumentNullException(nameof(Id), "An objective must have an id.");
        }
        if (Expectation is ObjectiveExpectation.NotMet && GetEffectiveResolutionBehavior() is ObjectiveResolutionBehavior.SelfResolution)
        {
            throw new InvalidOperationException("An objective whose expectation is NotMet cannot be self-resolving, because its success can only be established when the challenge settles it.");
        }

        ValidateParameters();
    }

    #endregion

    #region Helpers

    internal void Freeze()
    {
        _frozen = true;
    }

    protected abstract IChallengeObjective CreateObjective();

    protected void ValidateFrozen()
    {
        if (_frozen)
        {
            throw new InvalidOperationException("The parameters are frozen and can not be modified.");
        }
    }

    protected void SetNotRequired()
    {
        ValidateFrozen();
        _isRequired = false;
    }

    protected void SetName(string name)
    {
        ValidateFrozen();
        Name = name;
    }

    protected void SetDescription(string description)
    {
        ValidateFrozen();
        Description = description;
    }

    protected virtual void ValidateParameters()
    {
    }

    #endregion
}
