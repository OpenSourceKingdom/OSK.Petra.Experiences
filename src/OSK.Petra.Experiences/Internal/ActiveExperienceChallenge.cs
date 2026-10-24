using OSK.Petra.Experiences.Internal.Services;
using OSK.Petra.Experiences.Models;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Internal;

internal record ActiveExperienceChallenge(IExperienceChallenge Challenge, ObjectiveContext Context)
{
}
