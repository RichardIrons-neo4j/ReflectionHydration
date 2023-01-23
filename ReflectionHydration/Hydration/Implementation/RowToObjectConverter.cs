using System.Diagnostics.CodeAnalysis;
using Neo4j.Driver;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.Hydration.Implementation;

public class RowToObjectConverter : IRowToObjectConverter
{
    private readonly IObjectHydrationInfoBuilder _objectHydrationInfoBuilder;
    private readonly IEntityObjectInstanceManager _entityObjectInstanceManager;
    private readonly IEntityToObjectConverter _entityToObjectConverter;
    private readonly IObjectPropertyHydrator _objectPropertyHydrator;
    private readonly IObjectRelationshipHydrator _objectRelationshipHydrator;

    public RowToObjectConverter(
        IObjectHydrationInfoBuilder objectHydrationInfoBuilder,
        IEntityObjectInstanceManager entityObjectInstanceManager,
        IEntityToObjectConverter entityToObjectConverter,
        IObjectPropertyHydrator objectPropertyHydrator,
        IObjectRelationshipHydrator objectRelationshipHydrator)
    {
        _objectHydrationInfoBuilder = objectHydrationInfoBuilder;
        _entityObjectInstanceManager = entityObjectInstanceManager;
        _entityToObjectConverter = entityToObjectConverter;
        _objectPropertyHydrator = objectPropertyHydrator;
        _objectRelationshipHydrator = objectRelationshipHydrator;
    }

    public IEnumerable<T> ConvertRowsToObjects<T>(IEnumerable<IRecord> records) where T : new()
    {
        var objectHydrationInfo = _objectHydrationInfoBuilder.GetObjectHydrationInfo<T>();
        foreach (var record in records)
        {
            var rowObject = new T();

            _objectPropertyHydrator.HydrateObjectProperties(
                objectHydrationInfo,
                record,
                rowObject,
                _entityObjectInstanceManager);

            _objectRelationshipHydrator.HydrateObjectRelationships(
                objectHydrationInfo,
                record,
                rowObject,
                _entityObjectInstanceManager);

            yield return rowObject;
        }
    }
}
