using Microsoft.Extensions.Logging;
using ReflectionHydration.Hydration.Abstractions;

namespace ReflectionHydration.DemoStages.Stage2;

public class DemoStage2 : IDemoStage
{
    public int Stage => 2;

    private readonly ExampleQuery _exampleQuery;
    private readonly IColumnToObjectConverter _columnToObjectConverter;
    private readonly ILogger<DemoStage2> _logger;

    public DemoStage2(
        ExampleQuery exampleQuery,
        IColumnToObjectConverter columnToObjectConverter,
        ILogger<DemoStage2> logger)
    {
        _exampleQuery = exampleQuery;
        _columnToObjectConverter = columnToObjectConverter;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        var movies = _columnToObjectConverter.ConvertColumnToObjects<Movie>(records, "movie");
        foreach (var movie in movies)
        {
            _logger.LogDebug("{Movie}, ID: {Id}", movie, movie.GetHashCode());
        }
    }
}
