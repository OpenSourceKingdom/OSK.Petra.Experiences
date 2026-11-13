using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

public class ExperienceState
{
    #region Variables

    public ExperienceStatus Status { get; set; }

    public bool IsPinned { get; set; }

    public string? ActiveChallengeId { get; set; }

    public Dictionary<string, ChallengeState> ChallengeStates { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, object> Outputs { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    #endregion

    #region Api

    public ChallengeState? ActiveChallengeState
        => ActiveChallengeId is not null && ChallengeStates.TryGetValue(ActiveChallengeId, out var state)
            ? state
            : null;

    public ExperienceState Clone()
        => new()
        {
            Status = Status,
            IsPinned = IsPinned,
            ActiveChallengeId = ActiveChallengeId,
            ChallengeStates = ChallengeStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone(), StringComparer.OrdinalIgnoreCase),
            Outputs = Outputs
        };

    #endregion
}
