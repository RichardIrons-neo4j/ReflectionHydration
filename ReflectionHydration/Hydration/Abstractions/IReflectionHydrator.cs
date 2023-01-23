using Neo4j.Driver;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IReflectionHydrator
{
    IEnumerable<T> ConvertColumnToObjects<T>(IEnumerable<IRecord> records, string columnName) where T : new();
    IEnumerable<T> ConvertRowsToObjects<T>(IEnumerable<IRecord> records) where T : new();
}
