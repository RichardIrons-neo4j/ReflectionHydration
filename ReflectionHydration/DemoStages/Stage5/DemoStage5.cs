using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.DemoStages.Stage4;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.DemoStages.Stage5;

public class DemoStage5 : IDemoStage
{
    public int Stage => 5;

    private readonly ExampleQuery _exampleQuery;
    private readonly IRowToObjectConverter _rowToObjectConverter;
    private readonly ILogger<DemoStage5> _logger;

    public DemoStage5(
        ExampleQuery exampleQuery,
        IRowToObjectConverter rowToObjectConverter,
        ILogger<DemoStage5> logger)
    {
        _exampleQuery = exampleQuery;
        _rowToObjectConverter = rowToObjectConverter;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        var rows = _rowToObjectConverter.ConvertRowsToObjects<TestQueryRow>(records).ToList();

        var tomHanks = rows.Select(h => h.Person).First(p => p.Name == "Tom Hanks");
        
        _logger.LogDebug("Actor {Actor} has appeared in:", tomHanks.Name);
        foreach (var relation in tomHanks.GetRelations<ActedInRelationship>())
        {
            var movie = relation.As<Movie>();
            var otherActors = movie.GetRelations<ActedInRelationship>().Where(a => a != tomHanks);
            var costars = string.Join(", ", otherActors.Select(a => a.As<Person>().Name));
            var director = movie.GetRelations<DirectedRelationship>().First().As<Person>();
            _logger.LogDebug("{Movie} with {Costars}, directed by {Director}", movie.Title, costars, director.Name);
        }
    }
}
