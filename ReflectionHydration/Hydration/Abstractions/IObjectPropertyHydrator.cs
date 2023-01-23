using Neo4j.Driver;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IObjectPropertyHydrator
{
    void HydrateObjectProperties<T>(
        ObjectHydrationInfo objectHydrationInfo,
        IRecord record,
        T rowObject,
        IEntityObjectInstanceManager entityObjectInstanceManager);
}
