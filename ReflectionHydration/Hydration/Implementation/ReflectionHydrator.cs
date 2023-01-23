using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.Hydration.Implementation;

public class ReflectionHydrator : IReflectionHydrator
{
    private readonly ISetterMapFactory _setterMapFactory;
    private readonly INodeToObjectConverter _nodeToObjectConverter;
    private readonly IObjectHydrationInfoBuilder _objectHydrationInfoBuilder;
    private readonly ILogger<ReflectionHydrator> _logger;

    public ReflectionHydrator(
        ISetterMapFactory setterMapFactory,
        INodeToObjectConverter nodeToObjectConverter,
        IObjectHydrationInfoBuilder objectHydrationInfoBuilder,
        ILogger<ReflectionHydrator> logger)
    {
        _setterMapFactory = setterMapFactory;
        _nodeToObjectConverter = nodeToObjectConverter;
        _objectHydrationInfoBuilder = objectHydrationInfoBuilder;
        _logger = logger;
    }

    public IEnumerable<T> ConvertColumnToObjects<T>(IEnumerable<IRecord> records, string columnName) where T : new()
    {
        var setterMap = _setterMapFactory.GetSetterMap(typeof(T));
        var instances = new Dictionary<string, T>();
        foreach (var record in records)
        {
            var node = record[columnName].As<INode>();
            if (!instances.TryGetValue(node.ElementId, out var item))
            {
                item = _nodeToObjectConverter.CreateObjectFromNode<T>(node, setterMap);
                instances[node.ElementId] = item;
            }

            yield return item;
        }
    }

    public IEnumerable<T> ConvertRowsToObjects<T>(IEnumerable<IRecord> records) where T : new()
    {
        var info = _objectHydrationInfoBuilder.GetObjectHydrationInfo<T>();
        var instances = new Dictionary<string, object>();
        foreach (var record in records)
        {
            var rowObject = new T();
            foreach (var (nodeName, setterMap, propertyInfo) in info)
            {
                var node = record[nodeName].As<INode>();
                if (!instances.TryGetValue(node.ElementId, out var item))
                {
                    item = _nodeToObjectConverter.CreateObjectFromNode(propertyInfo.PropertyType, node, setterMap);
                    instances[node.ElementId] = item;
                }

                propertyInfo.SetMethod?.Invoke(rowObject, new[] { item });
            }

            yield return rowObject;
        }
    }
}
