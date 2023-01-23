using System.Reflection;
using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public class RelationshipToObjectPropertyMapBuilder : IRelationshipToObjectPropertyMapBuilder
{
    private readonly ISetterMapFactory _setterMapFactory;

    public RelationshipToObjectPropertyMapBuilder(
        ISetterMapFactory setterMapFactory)
    {
        _setterMapFactory = setterMapFactory;
    }

    private MethodInfo? GetRelationshipSetter<T>(Type propertyType) where T : Attribute
    {
        var property = propertyType.GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<T>() is not null);

        return property?.SetMethod;
    }

    public List<RecordRelationshipToObjectPropertyMap> GetRecordRelationshipToObjectPropertyMaps<T>()
    {
        var propsToSet = typeof(T).GetProperties()
            .Select(p => new { prop = p, attr = p.GetCustomAttribute<HydrateFromRelationshipAttribute>() })
            .Where(p => p.attr is not null)
            .Select(
                p => new
                {
                    p.attr!.ColumnName,
                    p.attr!.RelationshipType,
                    Info = p.prop
                });

        var maps = new Dictionary<Type, RelationshipToObjectMap>();
        var recordRelationshipToObjectPropertyMaps = new List<RecordRelationshipToObjectPropertyMap>();
        foreach (var prop in propsToSet)
        {
            var propertyType = prop.Info.PropertyType;
            if (!maps.TryGetValue(propertyType, out var relationshipToObjectMap))
            {
                var startSetter = GetRelationshipSetter<RelationshipStartAttribute>(propertyType);
                var endSetter = GetRelationshipSetter<RelationshipEndAttribute>(propertyType);
                var otherPropertiesMap = _setterMapFactory.GetSetterMap(propertyType);
                relationshipToObjectMap = new RelationshipToObjectMap(
                    prop.RelationshipType,
                    startSetter,
                    endSetter,
                    otherPropertiesMap);

                maps[propertyType] = relationshipToObjectMap;
            }

            recordRelationshipToObjectPropertyMaps.Add(
                new RecordRelationshipToObjectPropertyMap(prop.ColumnName, relationshipToObjectMap, prop.Info));
        }

        return recordRelationshipToObjectPropertyMaps;
    }
}
