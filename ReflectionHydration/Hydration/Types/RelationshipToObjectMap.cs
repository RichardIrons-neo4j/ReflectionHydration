using System.Reflection;

namespace ReflectionHydration.Hydration.Types;

public class RelationshipToObjectMap
{
    public string RelationshipType { get; }
    public MethodInfo? RelationshipStartSetter { get; }
    public MethodInfo? RelationshipEndSetter { get; }
    public NodeToObjectMap OtherPropertiesMap { get; }

    public RelationshipToObjectMap(
        string relationshipType,
        MethodInfo? relationshipStartSetter,
        MethodInfo? relationshipEndSetter,
        NodeToObjectMap otherPropertiesMap)
    {
        RelationshipType = relationshipType;
        RelationshipStartSetter = relationshipStartSetter;
        RelationshipEndSetter = relationshipEndSetter;
        OtherPropertiesMap = otherPropertiesMap;
    }
}
