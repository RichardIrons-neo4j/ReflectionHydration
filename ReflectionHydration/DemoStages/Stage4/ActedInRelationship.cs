using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.Hydration.Attributes;

namespace ReflectionHydration.DemoStages.Stage4;

public class ActedInRelationship
{
    [RelationshipStart]
    public Person Actor { get; private set; } = new();

    [RelationshipEnd]
    public Movie Movie { get; private set; } = new();

    [HydrateFromProperty("roles")]
    public List<string> Roles { get; private set; } = new();

    public override string ToString()
    {
        return $"{Actor} starred in {Movie} as {Roles[0]}";
    }
}
