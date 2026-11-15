using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Enumerators.Sequential;

/// <summary>
/// An enumerator for experiences that simply progress in a linear fashion
/// </summary>
/// <param name="challenges">The challenges for the experience</param>
public class SequentialChallengeEnumerator(IEnumerable<IExperienceChallenge> challenges) : IExperienceEnumerator
{
    #region Variables

    private int _index = -1;

    private readonly IReadOnlyList<IExperienceChallenge> _challenges = challenges?.ToArray() ?? throw new ArgumentNullException(nameof(challenges));

    #endregion

    #region IExperienceEnumerator
        
    public IExperienceChallenge? Current { get; private set; }

    public bool MoveNext(IExperienceTransitionContext context)
    {
        if (_index is -1)
        {
            _index = string.IsNullOrWhiteSpace(context.ActiveChallengeId)
                ? 0
                : challenges.Select((challenge, index) => (Challenge: challenge, Index: index))
                            .FirstOrDefault(tuple => tuple.Challenge.Id.Equals(context.ActiveChallengeId, StringComparison.OrdinalIgnoreCase)).Index;
        }

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
