namespace OSK.Petra.Experiences.Models;

public interface IChallengeObjective
{
    string Name { get; }

    string? Description { get; }

    bool IsRequired { get; }

    ObjectiveResolutionBehavior ResolutionBehavior { get; }

    void Resolve(IObjectiveContext context, ObjectiveResolutionInitiator initiator);
}