using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IRecordToObjectPropertyMapBuilder
{
    List<RecordNodeToObjectPropertyMap> GetRecordToObjectPropertyMaps<T>();
}
