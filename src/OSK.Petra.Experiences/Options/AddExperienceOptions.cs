using OSK.Petra.Experiences.Models;
using System.Collections.Generic;

namespace OSK.Petra.Experiences.Options;

public class AddExperienceOptions
{
    public ExperienceStatus? Status { get; set; }

    public bool IsPinned { get; set; }

    public string? ActiveChallengeId { get; set; }

    public IEnumerable<ChallengeState> ChallengeStates { get; set; } = [];

    public Dictionary<string, object> Outputs { get; set; } = [];
}
