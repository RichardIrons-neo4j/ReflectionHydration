using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.DemoStages.Stage2;

public class Movie : Relation
{
    [HydrateFromProperty]
    public string? Tagline { get; private set; }

    [HydrateFromProperty]
    public string? Title { get; private set; }

    [HydrateFromProperty]
    public long Released { get; private set; }

    public override string ToString()
    {
        return $"{Title} ({Released})";
    }
}
