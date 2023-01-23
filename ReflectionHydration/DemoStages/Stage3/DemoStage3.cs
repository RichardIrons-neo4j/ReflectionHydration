using Microsoft.Extensions.Logging;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Implementation;

namespace ReflectionHydration.DemoStages.Stage3;

public class DemoStage3 : IDemoStage
{
    public int Stage => 3;

    private readonly ExampleQuery _exampleQuery;
    private readonly IRowToObjectConverter _rowToObjectConverter;
    private readonly ILogger<DemoStage3> _logger;

    public DemoStage3(
        ExampleQuery exampleQuery,
        IRowToObjectConverter rowToObjectConverter,
        ILogger<DemoStage3> logger)
    {
        _exampleQuery = exampleQuery;
        _rowToObjectConverter = rowToObjectConverter;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        var rows = _rowToObjectConverter.ConvertRowsToObjects<TestQueryRow>(records);
        foreach (var row in rows)
        {
            _logger.LogDebug(
                "Movie: {Movie} ({MovieHashCode}), Person: {Person} ({PersonHashCode})",
                row.Movie,
                row.Movie.GetHashCode(),
                row.Person,
                row.Person.GetHashCode());
        }
    }
}
