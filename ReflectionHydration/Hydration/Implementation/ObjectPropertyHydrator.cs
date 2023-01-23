// Copyright (c) "Neo4j"

using Neo4j.Driver;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public class ObjectPropertyHydrator : IObjectPropertyHydrator
{
    public void HydrateObjectProperties<T>(
        ObjectHydrationInfo objectHydrationInfo,
        IRecord record,
        T rowObject,
        IEntityObjectInstanceManager entityObjectInstanceManager)
    {
        foreach (var (nodeName, nodeToObjectMap, propertyInfo) in objectHydrationInfo.RecordToObjectPropertyMaps)
        {
            var node = record[nodeName].As<INode>();
            var item = entityObjectInstanceManager.GetObjectForEntity(node, nodeToObjectMap);

            propertyInfo.SetMethod?.Invoke(rowObject, new[] { item });
        }
    }
}
