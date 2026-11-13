using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

public abstract class ObjectiveParameters
{
    #region Variables

    private string? _name;

    private bool _frozen;

    private bool _isRequired = true;

    public required string Id { get; init; }

    public string Name 
    { 
        get => string.IsNullOrWhiteSpace(_name) ? Id : _name;
        private set => _name = value;
    }

    public string Description { get; private set; } = string.Empty;

    public bool IsRequired 
    {
        get => _isRequired;
        init
        {
            _isRequired = value;
        } 
    } 

    public ObjectiveExpectation Expectation { get; init; } = ObjectiveExpectation.Met;

    public ObjectiveResolutionBehavior? ResolutionBehavior { get; init; }

    #endregion

    #region Api

    public abstract ICloneable? GetInitialValue();

    public IChallengeObjective ToObjective()
    {
        ValidateParameters();
        return CreateObjective();
    }

    public ObjectiveResolutionBehavior GetEffectiveResolutionBehavior()
        => ResolutionBehavior
           ?? (Expectation is ObjectiveExpectation.Met
                ? ObjectiveResolutionBehavior.SelfResolution
                : ObjectiveResolutionBehavior.ChallengeResolution);

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
