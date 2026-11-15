# OSK.Petra.Experiences

Core logic for handling experiences for an application. Experiences can be a single quest, quest line, campaign, or any other type of narrative for the application that a user can engage with. Experiences
are progressed through an experience enumerator, and the core services tracks various challenges and objectives for completion for each experience.

Experiences consist of challenges, which consist of objectives. Objectives are the goals or tasks for an experience that a user aims to complete. The service will track the state of each objective and determine
whether it has been completed and process the completion of the challenge and experience. Each objective also has a capability to add quick access outputs that can be referenced by experience enumerators
to determine the next step in the experience using previous decisions and outcomes.

Experience enumwerators are the focal point in determining what challenges are given to users and how experiences are navigated. The library provides a few implementations for enumerators:
- `Sequential` - Each challenge in the experience will occur in a provided order without need for external outputs and decisions by a user. A linear narrative arc and progression.

Objectives can be created by implementing the `IChallengeObjective` interface and an `IObjectiveTextFormatter` can be used to format the text for each objective for UI display. Objectives can be configured
to have different expected outcomes, e.g. reaching a threshold value or not reaching a threshold value. For example, a counter objective can be defined to have a limit of 3 lives, meaning that once it hits the limit,
it is failed. The expected outcome would be to not be met in this case. Another example would be for a user to mine 10 ores, where the expected outcome would be to meet the threshold value. Additionally, objectives can 
be configured to have different resolution behaviors - a resolution behavior defines how the objective manages completion. For example, a self resolution indicates to the system that the objective will
complete on its own, while a challenge resolution indicates that the challenge should resolve it when it is finished.

The library currently provides a few implementations of objectives:
- `CounterObjective` - An objective that uses a counter to determine whether requirements are met. This can be constructed using the `CounterParameters` class to specify the initial value, threshold value, comparison type, and other settings for the counter.
- `TimerObjective` - An objective that uses a timer to determine whether requirements are met. This can be constructed using the `TimerParameters` class to specify the timer duration and other settings for the timer.

The library will track all experiences that are added, so it is important to add every experience to the service, especially if it is desired that outputs of other experiences will be used to progress active challenge objectives.
There are various statuses available for an experience, including 'Available', 'Active', etc. Users will need to start or stop experiences using the returned `IExperienceContext` and can pin or unpin experiences through it.

The expected flow for the library:
 - Add the core dependencies using the `AddExperiences` call for the service collection
 - Add all experiences the user is able to use (at start, or during runtime). This can be configured to load previous saved states and progress.
 - Use the context to start/stop and engage with the experience
 - Trigger `IApplicationExperienceEvent` as needed and time ticks to the `IExperienceService`
 - Register event handlers to the experience service `OnExperienceEvent` hook, as the service will send update events through it