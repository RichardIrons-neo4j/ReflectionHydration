namespace ReflectionHydration.Hydration.Attributes;

public class HydrateFromRelationshipAttribute : Attribute
{
    public string ColumnName { get; }
    public string RelationshipType { get; }

    public HydrateFromRelationshipAttribute(string columnName, string relationshipType)
    {
        ColumnName = columnName;
        RelationshipType = relationshipType;
    }
}
