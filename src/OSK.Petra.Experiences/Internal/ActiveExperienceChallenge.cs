using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Internal;

internal class ActiveExperienceChallenge(IExperienceChallenge challenge, ObjectiveContext context, IEnumerable<ObjectiveDetails> details) : IChallengeDetails
{
    #region IChallengeDetails

    public IExperienceChallenge Challenge => challenge;

    public IReadOnlyList<IChallengeObjectiveDetails> Objectives { get; } = [.. details];

    public ChallengeStatus Status => context.ChallengeState.Status;

    public IChallengeObjectiveDetails this[string objectiveId]
        => Objectives.FirstOrDefault(o => o.Objective.Id.Equals(objectiveId, StringComparison.OrdinalIgnoreCase))
            ?? throw new KeyNotFoundException($"Objective with id '{objectiveId}' not found.");

    #endregion

    #region Api

    public bool ChallengeFailed { get; private set; }

    public void ForEach(Action<ObjectiveContext, ObjectiveDetails> action, bool allowBreakOnFail = false)
    {
        foreach (var details in details)
        {
            context.SetObjective(details.State.ObjectiveId);
            action(context, details);

            ChallengeFailed = ChallengeFailed || (details.Objective.IsRequired && context.ResolvedStatus is ObjectiveStatus.Failed);
        }
    }

    #endregion
}
