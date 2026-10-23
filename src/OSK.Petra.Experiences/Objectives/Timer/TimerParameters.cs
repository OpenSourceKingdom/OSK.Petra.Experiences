using System;

namespace OSK.Petra.Experiences.Objectives.Timer;

public class TimerParameters: ObjectiveParameters
{
    #region Static

    public static TimerParameters Duration(string name, TimeSpan timer, bool isRequired = true)
        => Duration(name, string.Empty, timer, isRequired);

    public static TimerParameters Duration(string name, string description, TimeSpan timer, bool isRequired = true)
        => Create(name, description, timer, isRequired, ObjectiveExpectation.Met);

    public static TimerParameters Alarm(string name, TimeSpan timer, bool isRequired = true)
        => Alarm(name, string.Empty, timer, isRequired);

    public static TimerParameters Alarm(string name, string description, TimeSpan timer, bool isRequired = true)
        => Create(name, description, timer, isRequired, ObjectiveExpectation.NotMet);

    internal static TimerParameters Create(string name, string description, TimeSpan timer, bool isRequired, ObjectiveExpectation expectation)
    {
        var parameters = new TimerParameters()
        {
            Name = name,
            Description = description,
            IsRequired = isRequired,
            Timer = timer,
            Expectation = expectation
        };

        parameters.Validate();

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

    #endregion
}
