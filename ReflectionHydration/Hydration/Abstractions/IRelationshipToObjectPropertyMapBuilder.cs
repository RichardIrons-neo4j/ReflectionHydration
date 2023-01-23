using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IRelationshipToObjectPropertyMapBuilder
{
    List<RecordRelationshipToObjectPropertyMap> GetRecordRelationshipToObjectPropertyMaps<T>();
}
