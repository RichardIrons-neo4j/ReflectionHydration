using Neo4j.Driver;

namespace TelemetryTest;

public class ExampleQuery
{
    private const string Query = "MATCH (movie:Movie) WHERE movie.title = 'The Matrix' RETURN movie";

    private readonly IDriver _driver;

    public ExampleQuery()
    {
        _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"),
            cfg => cfg.WithLogger(new Neo4jSerilogger()));
    }

    public async Task<List<IRecord>> GetRecordsUsingQueryExecuteAsync()
    {
        // run the query using the ExecuteAsync method ("exe")
        var queryExecution = await _driver
            .ExecutableQuery(Query)
            .ExecuteAsync();

        return queryExecution.Result.ToList();
    }

    public async Task<List<IRecord>> GetRecordsUsingSessionRunAsync()
    {
        // run the query using an auto-commit transaction ("run")
        await using var session = _driver.AsyncSession();
        var cursor = await session.RunAsync(Query);
        return await cursor.ToListAsync();
    }

    public async Task<List<IRecord>> GetRecordsUsingTransactionFunctionAsync()
    {
        // run the query using a transaction function ("fn")
        return await _driver.AsyncSession().ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(Query);
            return await cursor.ToListAsync();
        });
    }

    public async Task<List<IRecord>> GetRecordsUsingUnmanagedTransaction()
    {
        // run the query using SessionAsync.BeginTransactionAsync ("tx")
        await using var session = _driver.AsyncSession();
        var tx = await session.BeginTransactionAsync();
        var cursor = await tx.RunAsync(Query);
        var records = await cursor.ToListAsync();
        await tx.CommitAsync();
        return records;
    }
}
