using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.DemoStages.Stage2;

public class Person : Relation
{
    [HydrateFromProperty]
    public string Name { get; private set; } = string.Empty;

    [HydrateFromProperty]
    public long Born { get; private set; }

    public override string ToString()
    {
        return Name;
    }
}
