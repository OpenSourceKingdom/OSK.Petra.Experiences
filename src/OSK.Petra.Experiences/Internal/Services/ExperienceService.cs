using OSK.Petra.Experiences.Events;
using OSK.Petra.Experiences.Models;
using OSK.Petra.Experiences.Options;
using OSK.Petra.Experiences.Ports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Petra.Experiences.Internal.Services;

internal class ExperienceService(IExperienceContextProvider contextProvider) : IExperienceService
{
    #region Variables

    private readonly Dictionary<Guid, IInternalExperienceContext> _contextLookup = [];

    #endregion

    #region IExperienceService

    public event Action<IExperienceEvent>? OnExperienceEvent;

    public IExperienceContext Add(IExperience experience, AddExperienceOptions? options = null)
    {
        if (experience is null)
        {
            throw new ArgumentNullException(nameof(experience));
        }
        if (_contextLookup.TryGetValue(experience.Id, out var context))
        {
            return context;
        }

        context = contextProvider.GetExperienceContext(experience, new()
        {
            ActiveChallengeId = options?.ActiveChallengeId,
            IsPinned = options?.IsPinned ?? false,
            Status = options?.Status ?? ExperienceStatus.Available,
            ChallengeStates = options?.ChallengeStates?.ToDictionary(s => s.ChallengeId, s => s.Clone(), StringComparer.OrdinalIgnoreCase) ?? new(StringComparer.OrdinalIgnoreCase),
            Outputs = options?.Outputs ?? new(StringComparer.OrdinalIgnoreCase)
        });
        _contextLookup[experience.Id] = context;

        context.OnEvent += OnExperienceEvent;

        OnExperienceEvent?.Invoke(new ExperienceAddedEvent(context));

        if (context.Status is ExperienceStatus.Active)
        {
            context.Start();
        }

        return context;
    }

    public IExperienceContext? GetContext(Guid experienceId)
    {
        return _contextLookup.TryGetValue(experienceId, out var context)
            ? context
            : null;
    }

    public IEnumerable<IExperienceContext> GetContexts(ExperienceSearchOptions? searchOptions = null)
    {
        var searchableExperiences = searchOptions?.StatusFilter is null || searchOptions.Value.StatusFilter.Length is 0
            ? _contextLookup.Values
            : _contextLookup.Values.Where(context => searchOptions.Value.StatusFilter.Contains(context.Status));

        return searchOptions?.IsPinned is null
            ? searchableExperiences
            : searchableExperiences.Where(context => context.Pinned == searchOptions.Value.IsPinned);
    }

    public bool Remove(Guid experienceId)
    {
        if (_contextLookup.TryGetValue(experienceId, out var context))
        {
            context.OnEvent -= OnExperienceEvent;
            context.Dispose();

            _contextLookup.Remove(experienceId);
            OnExperienceEvent?.Invoke(new ExperienceRemovedEvent(context));

            return true;
        }

        return false;
    }

    public void Update(TimeSpan deltaTime)
    {
        foreach (var context in GetActiveExperiences())
        {
            context.Tick(deltaTime);
        }
    }

    public void ProcessEvent(IApplicationExperienceEvent appEvent)
    {
        if (appEvent is null)
        {
            throw new ArgumentNullException(nameof(appEvent));
        }

        foreach (var context in GetActiveExperiences())
        {
            context.ProcessEvent(appEvent);
        }
    }

    #endregion

    #region Helpers

    private IEnumerable<IInternalExperienceContext> GetActiveExperiences()
        => _contextLookup.Values.Where(context => context.Status is ExperienceStatus.Active);

    #endregion
}
