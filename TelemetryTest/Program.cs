// See https://aka.ms/new-console-template for more information

using Serilog;
using Serilog.Templates;
using TelemetryTest;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Filter.ByIncludingOnly("@m like '%TELEMETRY%'")
    .WriteTo.Console(new ExpressionTemplate("{@m}\n"))
    .CreateLogger();

var query = new ExampleQuery();

var exeTypes = new[] { "exe", "run", "fn", "tx" };
var metrics = new Dictionary<string, int>();
var tasks = new List<Task>();

for (int x = 0; x < 4; x++)
{
    metrics.Clear();
    tasks.Clear();

    for (int i = 0; i < 20; i++)
    {
        var exeType = exeTypes[Random.Shared.Next(0, 4)];
        IncremementCounter(metrics, exeType);
        tasks.Add(
            exeType switch
            {
                "exe" => query.GetRecordsUsingQueryExecuteAsync(),
                "run" => query.GetRecordsUsingSessionRunAsync(),
                "fn" => query.GetRecordsUsingTransactionFunctionAsync(),
                "tx" => query.GetRecordsUsingUnmanagedTransaction(),
                _ => throw new NotImplementedException()
            });
    }

    await Task.WhenAll(tasks);
    DisplayCounters(metrics);
}

///////////////////////////////////////////

void DisplayCounters(Dictionary<string, int> counters)
{
    Console.Write("Counters: ");
    foreach (var (key, value) in counters)
    {
        Console.Write($"<{key}: {value}> ");
    }

    Console.WriteLine();
}

void IncremementCounter(Dictionary<string, int> counters, string key)
{
    if (counters.ContainsKey(key))
    {
        counters[key]++;
    }
    else
    {
        counters[key] = 1;
    }
}
