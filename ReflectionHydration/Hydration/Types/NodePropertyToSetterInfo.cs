using System.Reflection;

namespace ReflectionHydration.Hydration.Types;

public record NodePropertyToSetterInfo(string NodeProperty, MethodInfo? Setter);
