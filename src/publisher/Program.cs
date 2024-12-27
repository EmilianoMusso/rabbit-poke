using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using poke.Services.Implementations;
using poke.Services.Interfaces;
using System.Threading.Tasks;

namespace poke;
public static class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IRabbitService, RabbitService>()
            .AddSingleton<ICmdLineService, CmdLineService>(_ => new CmdLineService(args))
            .BuildServiceProvider();

        var _logger = LoggerFactory
            .Create(builder => builder.AddConsole())
            .CreateLogger<Runner>();

        var _cmdLineService = serviceProvider.GetRequiredService<ICmdLineService>();
        var _rabbitService = serviceProvider.GetRequiredService<IRabbitService>();

        var runner = new Runner(_logger, _cmdLineService, _rabbitService);
        await runner.RunAsync();
    }
}