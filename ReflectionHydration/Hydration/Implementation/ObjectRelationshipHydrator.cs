using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Implementation;

public class ObjectRelationshipHydrator : IObjectRelationshipHydrator
{
    public void HydrateObjectRelationships<T>(
        ObjectHydrationInfo objectHydrationInfo,
        IRecord record,
        T rowObject,
        IEntityObjectInstanceManager entityObjectInstanceManager)
    {
        var propertiesToSet = objectHydrationInfo.RecordRelationshipToObjectPropertyMaps
            .Select(
                x => new
                {
                    x.NodeToObjectMap,
                    x.PropertyInfo,
                    Relationship = record[x.ColumnName].As<IRelationship>()
                })
            .Where(x => x.Relationship.Type == x.NodeToObjectMap.RelationshipType)
            .Select(x => (x.Relationship, x.NodeToObjectMap, x.PropertyInfo));

        foreach (var (relationship, relationshipToObjectMap, propertyInfo) in propertiesToSet)
        {
            var item = entityObjectInstanceManager.GetObjectForEntity(
                relationship,
                relationshipToObjectMap.OtherPropertiesMap);

            if (entityObjectInstanceManager.TryGetExistingObject(
                    relationship.StartNodeElementId,
                    out var startObj))
            {
                relationshipToObjectMap.RelationshipStartSetter?.Invoke(item, new[] { startObj });
            }

            if (entityObjectInstanceManager.TryGetExistingObject(
                    relationship.EndNodeElementId,
                    out var endObj))
            {
                relationshipToObjectMap.RelationshipEndSetter?.Invoke(item, new[] { endObj });

                (startObj as Relation)?.AddRelationship(item.GetType(), endObj!);
                if (startObj is not null)
                {
                    (endObj as Relation)?.AddRelationship(item.GetType(), startObj);
                }
            }

            propertyInfo.SetMethod?.Invoke(rowObject, new[] { item });
        }
    }
}
