using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Enumerators.Sequential;

public class SequentialChallengeEnumerator(IEnumerable<IExperienceChallenge> challenges, int? challengeIndex = null) : IExperienceEnumerator
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
        if (Current is null && _index >= 0 && _index < _challenges.Count)
        {
            Current = _challenges[_index];
            return true;
        }

        _index++;
        if (_challenges.Count is 0 || _index >= _challenges.Count)
        {
            Current = null;
            _index = _challenges.Count;
            return false;
        }

        Current = _challenges[_index];
        return true;
    }

    public void Reset()
    {
        Current = null;
        _index = -1;
    }

    public void Dispose()
    {
    }

    #endregion
}
