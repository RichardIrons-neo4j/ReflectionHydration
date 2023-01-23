using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.Hydration.Attributes;

namespace ReflectionHydration.DemoStages.Stage3;

public class TestQueryRow
{
    [HydrateFromNode]
    public Person Person { get; private set; } = null!;

    [HydrateFromNode]
    public Movie Movie { get; private set; } = null!;

    public override string ToString()
    {
        return $"Movie: {Movie}; Person {Person}";
    }
}
