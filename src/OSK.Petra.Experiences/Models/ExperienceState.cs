using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

public class ExperienceState
{
    #region Variables

    public ExperienceStatus Status { get; set; }

    public bool IsPinned { get; set; }

    public Dictionary<int, ChallengeState> ChallengeStates { get; set; } = [];

    public Dictionary<string, ICloneable> Outputs { get; set; } = [];

    #endregion

    #region Api

    public ExperienceState Clone()
        => new()
        {
            Status = Status,
            IsPinned = IsPinned,
            ChallengeStates = ChallengeStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone()),
            Outputs = Outputs.ToDictionary(kvp => kvp.Key, kvp => (ICloneable)kvp.Value.Clone())
        };

    #endregion
}
