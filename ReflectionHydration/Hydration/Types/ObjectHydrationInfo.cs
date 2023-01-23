namespace ReflectionHydration.Hydration.Types;

public class ObjectHydrationInfo
{
    public List<RecordNodeToObjectPropertyMap> RecordToObjectPropertyMaps { get; }
    public List<RecordRelationshipToObjectPropertyMap> RecordRelationshipToObjectPropertyMaps { get; }

    public ObjectHydrationInfo(
        IEnumerable<RecordNodeToObjectPropertyMap> recordToObjectPropertyMaps,
        IEnumerable<RecordRelationshipToObjectPropertyMap> recordRelationshipToObjectPropertyMaps)
    {
        RecordToObjectPropertyMaps = recordToObjectPropertyMaps.ToList();
        RecordRelationshipToObjectPropertyMaps = recordRelationshipToObjectPropertyMaps.ToList();
    }
}
