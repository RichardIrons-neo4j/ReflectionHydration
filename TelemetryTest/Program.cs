// See https://aka.ms/new-console-template for more information

using Serilog;
using TelemetryTest;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
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
        var exeType = Random.Shared.Next(0, 4);
        var type = exeTypes[exeType];
        IncremementCounter(metrics, type);
        tasks.Add(
            exeType switch
            {
                0 => query.GetRecordsUsingQueryExecuteAsync(), // exe
                1 => query.GetRecordsUsingSessionRunAsync(), // run
                2 => query.GetRecordsUsingTransactionFunctionAsync(), // fn
                3 => query.GetRecordsUsingUnmanagedTransaction(), // tx
                _ => throw new NotImplementedException()
            });

        await Task.WhenAll(tasks);
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
