using System;

namespace OSK.Petra.Experiences.Models;

public interface ITimeObjective: IChallengeObjective
{
    void ProcessTick(IObjectiveContext context, TimeSpan deltaTime);
}
