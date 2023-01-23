using System.Reflection;

namespace ReflectionHydration.Hydration.Types;

public record RecordNodeToObjectPropertyMap(
    string NodeName,
    NodeToObjectMap NodeToObjectMap,
    PropertyInfo PropertyInfo);
