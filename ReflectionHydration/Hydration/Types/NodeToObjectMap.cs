namespace ReflectionHydration.Hydration.Types;

public class NodeToObjectMap
{
    public Type DestinationType { get; }
    public List<NodePropertyToSetterInfo> NodePropertyToSetterInfos { get; }

    public NodeToObjectMap(Type destinationType, IEnumerable<NodePropertyToSetterInfo> nodePropertyToSetterInfos)
    {
        DestinationType = destinationType;
        NodePropertyToSetterInfos = nodePropertyToSetterInfos.ToList();
    }
}
