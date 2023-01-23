// Copyright (c) "Neo4j"

namespace ReflectionHydration.Hydration.Types;

public abstract class Relation
{
    private Dictionary<Type, List<object>> _hydratedRelationships = new();

    public void AddRelationship(Type relationType, object relation)
    {
        List<object>? list;
        if (!_hydratedRelationships.TryGetValue(relationType, out list))
        {
            list = new List<object>();
            _hydratedRelationships[relationType] = list;
        }

        list.Add(relation);
    }

    public IEnumerable<object> GetRelations<T>()
    {
        if (_hydratedRelationships.TryGetValue(typeof(T), out var list))
        {
            return list;
        }

        return Enumerable.Empty<object>();
    }
}
