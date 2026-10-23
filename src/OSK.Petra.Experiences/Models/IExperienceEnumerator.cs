using System;
using System.Diagnostics.CodeAnalysis;

namespace OSK.Petra.Experiences.Models;

public interface IExperienceEnumerator: IDisposable
{
    IExperienceChallenge? Current { get; }

    [MemberNotNullWhen(true, nameof(Current))]
    bool MoveNext(IExperienceTransitionContext context);

    void Reset();
}
