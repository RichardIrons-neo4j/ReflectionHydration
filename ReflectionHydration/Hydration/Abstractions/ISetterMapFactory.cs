using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface ISetterMapFactory
{
    NodeToObjectMap GetSetterMap(Type type);
}
