using System.Collections.Generic;

namespace OSK.Petra.Experiences.Models;

public interface IExperienceChallenge
{
    string Id { get; }

    string Name { get; }

    string? Description { get; }

    IReadOnlyList<IChallengeObjective> GetObjectives(IExperienceTransitionContext context);
}
