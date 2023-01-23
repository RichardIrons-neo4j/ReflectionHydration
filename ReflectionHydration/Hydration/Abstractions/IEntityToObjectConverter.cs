using Neo4j.Driver;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IEntityToObjectConverter
{
    object CreateObjectFromNode(IEntity node, NodeToObjectMap nodeToObjectMap, object? instance = null);

    T CreateObjectFromNode<T>(IEntity node, NodeToObjectMap setterMap) where T : new() =>
        (T)CreateObjectFromNode(node, setterMap, new T());
}
