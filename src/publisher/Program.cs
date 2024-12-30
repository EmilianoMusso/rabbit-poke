using poke.Services.Implementations;
using System.Threading.Tasks;

namespace poke;
public static class Program
{
    static async Task Main(string[] args)
    {
        var _cmdLineService = new CmdLineService(args);
        var _rabbitService = new RabbitService();

        var runner = new Runner(_cmdLineService, _rabbitService);
        await runner.RunAsync();
    }
}