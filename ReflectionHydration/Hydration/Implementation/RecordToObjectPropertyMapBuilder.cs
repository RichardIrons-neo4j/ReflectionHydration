using System.Reflection;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Implementation;

public class RecordToObjectPropertyMapBuilder : IRecordToObjectPropertyMapBuilder
{
    private readonly ISetterMapFactory _setterMapFactory;
    private Dictionary<Type, List<RecordNodeToObjectPropertyMap>> memo = new();

    public RecordToObjectPropertyMapBuilder(
        ISetterMapFactory setterMapFactory)
    {
        _setterMapFactory = setterMapFactory;
    }

    public List<RecordNodeToObjectPropertyMap> GetRecordToObjectPropertyMaps<T>()
    {
        if(memo.TryGetValue(typeof(T), out var cachedResult))
        {
            return cachedResult;
        }

        var propsToSet = typeof(T).GetProperties()
            .Select(p => new { prop = p, attr = p.GetCustomAttribute<HydrateFromNodeAttribute>() })
            .Where(p => p.attr is not null)
            .Select(
                p => new
                {
                    NodeName = p.attr!.NodeName ?? p.prop.Name.ToLower(),
                    Info = p.prop
                });

        var maps = new Dictionary<Type, NodeToObjectMap>();
        var recordToObjectPropertyMaps = new List<RecordNodeToObjectPropertyMap>();
        foreach (var prop in propsToSet)
        {
            var propertyType = prop.Info.PropertyType;
            if (!maps.TryGetValue(propertyType, out var map))
            {
                map = _setterMapFactory.GetSetterMap(propertyType);
                maps[propertyType] = map;
            }

            recordToObjectPropertyMaps.Add(new RecordNodeToObjectPropertyMap(prop.NodeName, map, prop.Info));
        }

        memo[typeof(T)] = recordToObjectPropertyMaps;

        return recordToObjectPropertyMaps;
    }
}
