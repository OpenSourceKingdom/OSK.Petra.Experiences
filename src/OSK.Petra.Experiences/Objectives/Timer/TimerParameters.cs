using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

public class TimerParameters: ObjectiveParameters<TimerParameters, TimerState>
{
    #region Static

    public static TimerParameters Duration(string id, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Duration(id, string.Empty, timer, configure);

    public static TimerParameters Duration(string id, string description, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Create(id, description, timer, ObjectiveExpectation.Met, configure);

    public static TimerParameters Alarm(string id, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Alarm(id, string.Empty, timer, configure);

    public static TimerParameters Alarm(string id, string description, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Create(id, description, timer, ObjectiveExpectation.NotMet, configure);

    internal static TimerParameters Create(string id, string description, TimeSpan timer, ObjectiveExpectation expectation, Action<TimerParameters>? configure = null)
    {
        var parameters = new TimerParameters()
        {
            Id = id,
            Timer = timer,
            Expectation = expectation
        };

        configure?.Invoke(parameters);

        parameters.Validate();
        parameters.Freeze();

        return parameters;
    }

    #endregion

    #region Variables

    public TimeSpan Timer { get; init; }

    #endregion

    #region Helpers

    protected override void ValidateParameters()
    {
        if (Timer <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("A timer can not be less than or equal to zero.");
        }
    }

    protected override IChallengeObjective CreateObjective()
        => new TimerObjective(this);

    protected override TimerState GetInitialState()
        => Timer;

    #endregion
}
