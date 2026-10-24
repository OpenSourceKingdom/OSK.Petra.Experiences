using OSK.Petra.Experiences.Enumerators.Sequential;
using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.UnitTests._Helpers;

public class TestExperience() : IExperience
{
    private readonly List<IExperienceChallenge> _challenges = [];

    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; set; } = "Test Experience";

    public string? Description { get; set; }

    public bool IsRequired { get; init; } = true;

    public bool IsSecret { get; init; } = false;

    public bool AllowStop { get; init; } = true;

    public ExperienceStopBehavior StopBehavior { get; init; } = ExperienceStopBehavior.Pause;

    public IExperienceEnumerator GetEnumerator(int challengeId = 0)
    {
        return new SequentialChallengeEnumerator(_challenges, challengeId);
    }
}
