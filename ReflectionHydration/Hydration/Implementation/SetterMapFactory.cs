using System.Reflection;
using Microsoft.Extensions.Logging;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Attributes;
using ReflectionHydration.Hydration.Types;

namespace ReflectionHydration.Hydration.Implementation;

public class SetterMapFactory : ISetterMapFactory
{
    private readonly ILogger<SetterMapFactory> _logger;

    public SetterMapFactory(ILogger<SetterMapFactory> logger)
    {
        _logger = logger;
    }

    public NodeToObjectMap GetSetterMap(Type type)
    {
        _logger.LogTrace("Building setter map for type {Type}", type.Name);
        return new NodeToObjectMap(
            type,
            type.GetProperties()
                .Select(p => new { prop = p, attr = p.GetCustomAttribute<HydrateFromPropertyAttribute>() })
                .Where(p => p.attr is not null)
                .Select(
                    p =>
                        new
                        {
                            Setter = p.prop.SetMethod,
                            Attribute = p.attr,
                            NodeProperty = p.attr?.PropertyName ?? p.prop.Name.ToLower()
                        })
                .Select(p => new NodePropertyToSetterInfo(p.NodeProperty, p.Setter)));
    }
}
