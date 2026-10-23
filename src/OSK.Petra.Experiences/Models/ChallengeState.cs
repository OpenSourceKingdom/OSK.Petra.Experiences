using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Models;

public class ChallengeState
{
    #region Variables

    public int ChallengeId { get; set; }

    public ChallengeStatus Status { get; set; }

    public Dictionary<int, ObjectiveState> ObjectiveStates { get; set; } = [];

    #endregion

    #region Api

    public ChallengeState Clone()
        => new()
        {
            ChallengeId = ChallengeId,
            Status = Status,
            ObjectiveStates = ObjectiveStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone())
        };

    #endregion
}