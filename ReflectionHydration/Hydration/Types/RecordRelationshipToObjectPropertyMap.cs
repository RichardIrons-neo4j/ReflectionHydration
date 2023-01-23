using System.Reflection;

namespace ReflectionHydration.Hydration.Types;

public record  RecordRelationshipToObjectPropertyMap(
    string ColumnName,
    RelationshipToObjectMap NodeToObjectMap,
    PropertyInfo PropertyInfo);
