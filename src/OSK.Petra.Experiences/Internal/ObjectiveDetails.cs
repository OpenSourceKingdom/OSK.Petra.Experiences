using OSK.Petra.Experiences.Models;

namespace OSK.Petra.Experiences.Internal;

internal class ObjectiveDetails(IChallengeObjective objective, ObjectiveState state) : IChallengeObjectiveDetails
{
    #region Variables

    internal ObjectiveState State => state;

    #endregion

    #region IChallengeObjectiveDetails

    public IChallengeObjective Objective => objective;

    public ObjectiveStatus Status
    {
        get => state.Status;
        set => state.Status = value;
    }

    public string GetDisplayText()
        => objective.GetValueText(state);

    #endregion
}
