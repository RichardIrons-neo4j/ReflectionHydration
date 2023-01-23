using Neo4j.Driver;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IRowToObjectConverter
{
    IEnumerable<T> ConvertRowsToObjects<T>(IEnumerable<IRecord> records) where T : new();
}
