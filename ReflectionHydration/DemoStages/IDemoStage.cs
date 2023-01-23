namespace ReflectionHydration.DemoStages;

public interface IDemoStage
{
    int Stage { get; }
    Task RunAsync();
}
