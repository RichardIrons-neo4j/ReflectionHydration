using Microsoft.Extensions.Logging;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.DemoStages.Stage4;

public class DemoStage4 : IDemoStage
{
    public int Stage => 4;

    private readonly ExampleQuery _exampleQuery;
    private readonly IRowToObjectConverter _rowToObjectConverter;
    private readonly ILogger<DemoStage4> _logger;

    public DemoStage4(
        ExampleQuery exampleQuery,
        IRowToObjectConverter rowToObjectConverter,
        ILogger<DemoStage4> logger)
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
            _logger.LogDebug("{Row}", row);
        }
    }
}
