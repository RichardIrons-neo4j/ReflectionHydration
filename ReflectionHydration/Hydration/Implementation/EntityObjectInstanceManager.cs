using System.Diagnostics.CodeAnalysis;
using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Implementation;

public class EntityObjectInstanceManager : IEntityObjectInstanceManager
{
    private readonly IEntityToObjectConverter _entityToObjectConverter;
    private Dictionary<string, object> _instances = new();

    public EntityObjectInstanceManager(IEntityToObjectConverter entityToObjectConverter)
    {
        _entityToObjectConverter = entityToObjectConverter;
    }

    public object GetObjectForEntity(IEntity entity, NodeToObjectMap nodeToObjectMap)
    {
        if(!_instances.TryGetValue(entity.ElementId, out var result))
        {
            result = _entityToObjectConverter.CreateObjectFromNode(
                entity,
                nodeToObjectMap);

            _instances[entity.ElementId] = result;
        }

        return result;
    }

    public bool TryGetExistingObject(string elementId, [NotNullWhen(true)] out object? existingObject)
    {
        return _instances.TryGetValue(elementId, out existingObject);
    }
}
