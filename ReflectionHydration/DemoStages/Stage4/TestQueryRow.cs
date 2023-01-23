using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.Hydration.Attributes;

namespace ReflectionHydration.DemoStages.Stage4;

public class TestQueryRow
{
    [HydrateFromNode]
    public Person Person { get; private set; } = null!;

    [HydrateFromNode]
    public Movie Movie { get; private set; } = null!;

    [HydrateFromRelationship("relationship", "ACTED_IN")]
    public ActedInRelationship? ActedInRelationship { get; private set; }

    [HydrateFromRelationship("relationship", "DIRECTED")]
    public DirectedRelationship? DirectedRelationship { get; private set; }

    public override string ToString()
    {
        return (ActedInRelationship as object ?? DirectedRelationship as object)?.ToString()
         ?? $"Unknown relationship between {Movie} and {Person}";
    }
}
