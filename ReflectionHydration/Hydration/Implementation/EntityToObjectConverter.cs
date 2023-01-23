using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Extensions;
using ReflectionHydration.Hydration.Types;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ReflectionHydration.Hydration.Implementation;

public class EntityToObjectConverter : IEntityToObjectConverter
{
    private readonly ISetterMapFactory _setterMapFactory;
    private readonly ILogger<EntityToObjectConverter> _logger;

    public EntityToObjectConverter(
        ISetterMapFactory setterMapFactory,
        ILogger<EntityToObjectConverter> logger)
    {
        _setterMapFactory = setterMapFactory;
        _logger = logger;
    }

    public object CreateObjectFromNode(
        IEntity node,
        NodeToObjectMap nodeToObjectMap,
        object? instance = null)
    {
        var result = instance ?? Activator.CreateInstance(nodeToObjectMap.DestinationType)!;

        foreach (var prop in nodeToObjectMap.NodePropertyToSetterInfos)
        {
            var value = node[prop.NodeProperty];
            if (value is not null)
            {
                value = value.As(prop.Setter!.GetParameters()[0].ParameterType);
                prop.Setter?.Invoke(result, new[] { value });
            }
        }

        _logger.LogTrace(
            "Created {Type} ({HashCode})",
            result.GetType().Name,
            result.GetHashCode());

        return result!;
    }
}
