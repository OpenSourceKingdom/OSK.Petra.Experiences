using System.Collections.Generic;

namespace OSK.Petra.Experiences.Models;

public interface IChallengeDetails
{
    IExperienceChallenge Challenge { get; }

    IReadOnlyList<IChallengeObjectiveDetails> Objectives { get; }

    ChallengeStatus Status { get; }

    IChallengeObjectiveDetails this[string objectiveId] { get; }
}