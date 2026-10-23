using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Enumerators.Sequential;

public class SequentialEnumerator(IEnumerable<IExperienceChallenge> challenges, int? challengeIndex = null) : IExperienceEnumerator
{
    #region Variables

    private int _index = challengeIndex.HasValue
        ? challengeIndex < 0 ? throw new ArgumentException(nameof(challengeIndex), "Challenge index must be greater than or equal to 0") : challengeIndex.Value
        : -1;

    private readonly IReadOnlyList<IExperienceChallenge> _challenges = challenges?.ToArray() ?? throw new ArgumentNullException(nameof(challenges));

    #endregion

    #region IExperienceEnumerator
        
    public IExperienceChallenge? Current { get; private set; }

    public bool MoveNext(IExperienceTransitionContext context)
    {
        if (_index >= _challenges.Count)
        {
            return false;
        }

        if (Current is null && _index >= 0)
        {
            Current = _challenges[_index];
            return true;
        }

        _index++;

        Current = _challenges[_index];
        return true;
    }

    public void Reset()
    {
        _index = -1;
    }

    public void Dispose()
    {
    }

    #endregion
}
