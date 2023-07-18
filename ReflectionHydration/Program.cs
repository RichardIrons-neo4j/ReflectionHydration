using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;
using ReflectionHydration.DemoStages;
using ReflectionHydration.DemoStages.Stage1;
using ReflectionHydration.DemoStages.Stage2;
using ReflectionHydration.DemoStages.Stage3;
using ReflectionHydration.DemoStages.Stage4;
using ReflectionHydration.DemoStages.Stage5;
using ReflectionHydration.Hydration.Abstractions;
using ReflectionHydration.Hydration.Implementation;
using Serilog;
using TestQueryRow = ReflectionHydration.DemoStages.Stage4.TestQueryRow;

namespace ReflectionHydration;

public class Benchmarks
{
    private IRowToObjectConverter? _rowToObjectConverter;
    private List<IRecord>? _records;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var serviceProvider = Program.BuildServices();
        _records = serviceProvider.GetRequiredService<ExampleQuery>().GetRecordsAsync().GetAwaiter().GetResult();
        _rowToObjectConverter = serviceProvider.GetRequiredService<IRowToObjectConverter>();
    }

    [Benchmark]
    public void DoHydration()
    {
        _rowToObjectConverter.ConvertRowsToObjects<TestQueryRow>(_records).ToList();
    }
}

public class Program
{
    public static IServiceProvider BuildServices()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        var services = new ServiceCollection()
            .AddSingleton<ExampleQuery>()
            .AddLogging(l => l.AddSerilog())
            .AddTransient<ISetterMapFactory, SetterMapFactory>()
            .AddTransient<IEntityToObjectConverter, EntityToObjectConverter>()
            .AddTransient<IObjectHydrationInfoBuilder, ObjectHydrationInfoBuilder>()
            .AddTransient<IEntityObjectInstanceManager, EntityObjectInstanceManager>()
            .AddTransient<IColumnToObjectConverter, ColumnToObjectConverter>()
            .AddTransient<IRowToObjectConverter, RowToObjectConverter>()
            .AddTransient<IObjectHydrationInfoBuilder, ObjectHydrationInfoBuilder>()
            .AddTransient<IRecordToObjectPropertyMapBuilder, RecordToObjectPropertyMapBuilder>()
            .AddTransient<IRelationshipToObjectPropertyMapBuilder, RelationshipToObjectPropertyMapBuilder>()
            .AddTransient<IObjectPropertyHydrator, ObjectPropertyHydrator>()
            .AddTransient<IObjectRelationshipHydrator, ObjectRelationshipHydrator>()
            .AddTransient<IDemoStage, DemoStage1>()
            .AddTransient<IDemoStage, DemoStage2>()
            .AddTransient<IDemoStage, DemoStage3>()
            .AddTransient<IDemoStage, DemoStage4>()
            .AddTransient<IDemoStage, DemoStage5>();

        return services.BuildServiceProvider();
    }

    public static async Task Main()
    {
        IServiceProvider serviceProvider = BuildServices();
        var demoStages = serviceProvider.GetRequiredService<IEnumerable<IDemoStage>>().ToList();
        int stage = 0;
        while(true)
        {
            string input;
            do
            {
                Console.Write("Stage: ");
                input = Console.ReadLine()!;
            } while (!(int.TryParse(input, out stage) && stage is >= 0 and <= 5));

            if (stage == 0)
            {
                break;
            }

            var demoStage = demoStages.First(d => d.Stage == stage);
            await demoStage.RunAsync();
        };
    }
}
