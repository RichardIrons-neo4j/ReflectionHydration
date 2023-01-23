using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.Hydration.Attributes;

namespace ReflectionHydration.DemoStages.Stage4;

public class DirectedRelationship
{
    [RelationshipStart]
    public Person Director { get; private set; } = new();

    [RelationshipEnd]
    public Movie Movie { get; private set; } = new();

    public override string ToString()
    {
        return $"{Director} directed {Movie}";
    }
}
