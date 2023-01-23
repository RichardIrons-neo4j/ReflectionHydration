using Neo4j.Driver;

namespace ReflectionHydration.Hydration.Extensions;

public static class HydrationExtensions
{
    public static object? As(this object value, Type desiredType)
    {
        var asMethod = typeof(ValueExtensions).GetMethods()
            .First(m => m.IsGenericMethod && m.GetParameters().Length == 1 && m.GetGenericArguments().Length == 1)
            .MakeGenericMethod(desiredType);

        return asMethod.Invoke(null, new[] { value });
    }
}
