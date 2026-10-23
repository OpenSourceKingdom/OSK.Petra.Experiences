using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives;

public class ObjectiveParameters
{
    #region Variables

    public required string Name { get; init; }

    public string Description { get; init; } = string.Empty;

    public bool IsRequired { get; init; } = true;

    public ObjectiveExpectation Expectation { get; init; } = ObjectiveExpectation.Met;

    public ObjectiveResolutionBehavior? ResolutionBehavior { get; init; }

    #endregion

    #region Api

    public ObjectiveResolutionBehavior GetEffectiveResolutionBehavior()
        => ResolutionBehavior
           ?? (Expectation is ObjectiveExpectation.Met
                ? ObjectiveResolutionBehavior.SelfResolution
                : ObjectiveResolutionBehavior.ChallengeResolution);

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(nameof(Name), "An objective must have a name.");
        }
        if (Expectation is ObjectiveExpectation.NotMet && GetEffectiveResolutionBehavior() is ObjectiveResolutionBehavior.SelfResolution)
        {
            throw new InvalidOperationException("An objective whose expectation is NotMet cannot be self-resolving, because its success can only be established when the challenge settles it.");
        }

        ValidateParameters();
    }

    #endregion

    #region Helpers

    protected virtual void ValidateParameters()
    {
    }

    #endregion
}
