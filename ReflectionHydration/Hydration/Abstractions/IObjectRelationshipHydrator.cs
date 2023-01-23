// Copyright (c) "Neo4j"

using Neo4j.Driver;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IObjectRelationshipHydrator
{
    void HydrateObjectRelationships<T>(
        ObjectHydrationInfo objectHydrationInfo,
        IRecord record,
        T rowObject,
        IEntityObjectInstanceManager entityObjectInstanceManager);
}
