using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IObjectHydrationInfoBuilder
{
    ObjectHydrationInfo GetObjectHydrationInfo<T>();
}
