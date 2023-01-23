using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.Hydration.Implementation;

public class ColumnToObjectConverter : IColumnToObjectConverter
{
    private readonly ISetterMapFactory _setterMapFactory;
    private readonly IEntityObjectInstanceManager _entityObjectInstanceManager;

    public ColumnToObjectConverter(
        ISetterMapFactory setterMapFactory,
        IEntityObjectInstanceManager entityObjectInstanceManager)
    {
        _setterMapFactory = setterMapFactory;
        _entityObjectInstanceManager = entityObjectInstanceManager;
    }

    public IEnumerable<T> ConvertColumnToObjects<T>(IEnumerable<IRecord> records, string columnName) where T : new()
    {
        var nodeToObjectMap = _setterMapFactory.GetSetterMap(typeof(T));
        foreach (var record in records)
        {
            yield return (T)_entityObjectInstanceManager.GetObjectForEntity(record[columnName].As<INode>(), nodeToObjectMap);
        }
    }
}
