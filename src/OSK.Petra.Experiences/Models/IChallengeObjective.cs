using System;

namespace OSK.Petra.Experiences.Models;

public interface IChallengeObjective
{
    string Id { get; }

    string Name { get; }

    string? Description { get; }

    bool IsRequired { get; }

    ObjectiveResolutionBehavior ResolutionBehavior { get; }

    ICloneable? GetInitialValue();

    string GetValueText(ObjectiveState state);

    void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator);
}