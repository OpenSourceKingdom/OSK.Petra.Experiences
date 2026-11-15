using OSK.Petra.Experiences.Models;
using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

public class TimerParameters: ObjectiveParameters<TimerParameters, TimerData>
{
    #region Static

    /// <summary>
    /// Creates a timer that is successful if the timer completes after the given time
    /// </summary>
    /// <param name="id">The unique id for the objective</param>
    /// <param name="timer">The amount of time for the timer to run</param>
    /// <param name="configure">An action for extra configuration of the timer</param>
    /// <returns>The parameters</returns>
    public static TimerParameters Duration(string id, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Duration(id, string.Empty, timer, configure);

    /// <summary>
    /// Creates a timer that is successful if the timer completes after the given time
    /// </summary>
    /// <param name="id">The unique id for the objective</param>
    /// <param name="description">Insightful text for the purpose of the timer</param>
    /// <param name="timer">The amount of time for the timer to run</param>
    /// <param name="configure">An action for extra configuration of the timer</param>
    /// <returns>The parameters</returns>
    public static TimerParameters Duration(string id, string description, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Create(id, description, timer, ObjectiveExpectation.Met, configure);

    /// <summary>
    /// Creates a timer that is failed if the timer completes after the given time
    /// </summary>
    /// <param name="id">The unique id for the objective</param>
    /// <param name="timer">The amount of time for the timer to run</param>
    /// <param name="configure">An action for extra configuration of the timer</param>
    /// <returns>The parameters</returns>
    public static TimerParameters Alarm(string id, TimeSpan timer, Action<TimerParameters>? configure = null)
        => Alarm(id, string.Empty, timer, configure);

    /// <summary>
    /// Creates a timer that is failed if the timer completes after the given time
    /// </summary>
    /// <param name="id">The unique id for the objective</param>
    /// <param name="description">Insightful text for the purpose of the timer</param>
    /// <param name="timer">The amount of time for the timer to run</param>
    /// <param name="configure">An action for extra configuration of the timer</param>
    /// <returns>The parameters</returns>
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

    /// <summary>
    /// The amount of time the timer starts with
    /// </summary>
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

    protected override TimerData GetInitialData()
        => Timer;

    #endregion
}
