using Neo4j.Driver;

namespace ReflectionHydration.Hydration.Abstractions;

public interface IColumnToObjectConverter
{
    IEnumerable<T> ConvertColumnToObjects<T>(IEnumerable<IRecord> records, string columnName) where T : new();
}
