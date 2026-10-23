using OSK.Petra.Experiences.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Challenges.Simple;

public class SimpleChallenge : IExperienceChallenge
{
    #region Variables

    private readonly IReadOnlyList<IChallengeObjective> _objectives;

    #endregion

    #region Constructors

    public SimpleChallenge(int id, string name, IEnumerable<IChallengeObjective> objectives)
        : this(id, name, string.Empty, objectives)
    {
    }

    public SimpleChallenge(int id, string name, string description, IEnumerable<IChallengeObjective> objectives)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
        Description = description;
        _objectives = objectives?.ToArray() ?? throw new ArgumentNullException(nameof(objectives));
    }

    #endregion

    #region IExperienceChallenge

    public int Id { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<IChallengeObjective> GetObjectives()
        => _objectives;

    #endregion
}
