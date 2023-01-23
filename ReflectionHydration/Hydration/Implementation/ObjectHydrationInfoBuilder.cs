using System.Reflection;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Implementation;

public class ObjectHydrationInfoBuilder : IObjectHydrationInfoBuilder
{
    private readonly ISetterMapFactory _setterMapFactory;
    private readonly IRecordToObjectPropertyMapBuilder _recordToObjectPropertyMapBuilder;
    private readonly IRelationshipToObjectPropertyMapBuilder _relationshipToObjectPropertyMapBuilder;

    private readonly Dictionary<Type, ObjectHydrationInfo> _cache = new();

    public ObjectHydrationInfoBuilder(
        ISetterMapFactory setterMapFactory,
        IRecordToObjectPropertyMapBuilder recordToObjectPropertyMapBuilder,
        IRelationshipToObjectPropertyMapBuilder relationshipToObjectPropertyMapBuilder)
    {
        _setterMapFactory = setterMapFactory;
        _recordToObjectPropertyMapBuilder = recordToObjectPropertyMapBuilder;
        _relationshipToObjectPropertyMapBuilder = relationshipToObjectPropertyMapBuilder;
    }

    public ObjectHydrationInfo GetObjectHydrationInfo<T>()
    {
        if(_cache.TryGetValue(typeof(T), out var result))
        {
            return result;
        }

        var nodeMaps = _recordToObjectPropertyMapBuilder.GetRecordToObjectPropertyMaps<T>();
        var relationshipMaps = _relationshipToObjectPropertyMapBuilder.GetRecordRelationshipToObjectPropertyMaps<T>();

        result = new ObjectHydrationInfo(nodeMaps, relationshipMaps);
        _cache[typeof(T)] = result;
        return result;
    }
}
