using Bogus;
using EdgeDbDemo.Data;
using EdgeDbDemo.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Spectre.Console;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddJsonFile("appsettings.json", false, true)
    .Build();

var services = new ServiceCollection()
    .AddLogging(logger =>
    {
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        logger
            .AddSerilog(serilogLogger, dispose: true);
    })
    .AddHttpClient<EdgeDbClient>(c =>
    {
        var host = config["EdgeDb:EdgeDb:Host"];
        var port = config["EdgeDb:EdgeDb:Port"];
        var database = config["EdgeDb:EdgeDb:Database"];

        c.BaseAddress = new Uri($"{host}:{port}/db/{database}/edgeql");
    });

using var serviceProvider = services.Services.BuildServiceProvider();

var edgeDbClient = serviceProvider.GetRequiredService<EdgeDbClient>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

var faker = new Faker<TODOModel>()
    .RuleFor(p => p.Title, f => f.Lorem.Sentence())
    .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
    .RuleFor(p => p.State, f => f.Random.Enum<TODOState>());

if (AnsiConsole.Confirm("Would you like to create a new TODO note?", false))
{
    var todoItem = faker.Generate();

    var query = "insert TODO { title := <str>$title, description := <str>$description, state := <State>$state }";
    var bindings = new Dictionary<string, object>
    {
        { "title", todoItem.Title! },
        { "description", todoItem.Description! },
        { "state", todoItem.State.ToStringFast() }
    };

    await edgeDbClient.ExecuteAsync(query, bindings);
}

var result = await edgeDbClient.QueryAsync<TODOModel>("select TODO { title, description, state, date_created }");

logger.LogInformation("{@Result}", result);
