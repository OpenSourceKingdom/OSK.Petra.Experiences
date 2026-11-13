namespace OSK.Petra.Experiences.Models;

public interface IChallengeObjectiveDetails
{
    IChallengeObjective Objective { get; }

    ObjectiveStatus Status { get; }

    string GetDisplayText();
}
