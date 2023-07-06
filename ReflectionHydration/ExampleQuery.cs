using Neo4j.Driver;

namespace ReflectionHydration;

public class ExampleQuery
{
    private readonly IDriver _driver;
    private List<IRecord>? _cache;

    public ExampleQuery()
    {
        _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"),
            cfg => cfg.WithLogger(new Neo4jSerilogger()));
    }

    public async Task<List<IRecord>> GetRecordsAsync()
    {
        if (_cache is not null)
        {
            return _cache;
        }

        const string query = """
            MATCH (movie:Movie)<-[relationship:ACTED_IN|DIRECTED]-(person:Person)
            WHERE movie.title =~ '(The|A) .*'
            RETURN movie, relationship, person
            """;

        var queryExecution = await _driver
            .ExecutableQuery(query)
            .ExecuteAsync();

        return queryExecution.Result.ToList();

    }
}
