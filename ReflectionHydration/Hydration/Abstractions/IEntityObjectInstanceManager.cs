using Neo4j.Driver;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IEntityObjectInstanceManager
{
    object GetObjectForEntity(IEntity entity, NodeToObjectMap nodeToObjectMap);
    bool TryGetExistingObject(string elementId, out object? existingObject);
}
