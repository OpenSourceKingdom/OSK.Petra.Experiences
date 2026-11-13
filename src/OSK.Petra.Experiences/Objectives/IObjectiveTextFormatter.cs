namespace OSK.Petra.Experiences.Objectives;

public interface IObjectiveTextFormatter<TParameters, TState>
    where TParameters: ObjectiveParameters
{
    string Format(TParameters parameters, TState state);
}
