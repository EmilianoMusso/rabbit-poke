using poke.Models;
using poke.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace poke;
public class Runner(ICmdLineService cmdLineService, IRabbitService rabbitService)
{
    private readonly ICmdLineService _cmdLineService = cmdLineService;
    private readonly IRabbitService _rabbitService = rabbitService;

    public async Task RunAsync()
    {
        await _cmdLineService.Process();
        await RunProcess(_cmdLineService.Options);
    }

    private async Task RunProcess(CmdLineOptions options)
    {
        await PublishMessage(options);
    }

    private async Task PublishMessage(CmdLineOptions options)
    {
        if (AreParametersInvalid(options))
        {
            await Console.Out.WriteLineAsync("One or more mandatory arguments was not specified.\nCheck for the following parameters:\n--connection\n--type\n--message");
            return;
        }

        var message = Message.FromOptions(options);

        await _rabbitService.PublishAsync(message);
        await Console.Out.WriteLineAsync($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Event '{options.TypeQueue}' published on bus");

        if (!string.IsNullOrEmpty(options.ReplyTo))
        {
            var _waitSeconds = options.WaitSeconds > 0 ? options.WaitSeconds : 15;
            await Console.Out.WriteLineAsync($"Waiting response for {_waitSeconds} seconds");
            Thread.Sleep(_waitSeconds * 1000);
        }
    }

    private static bool AreParametersInvalid(CmdLineOptions options)
    {
        return string.IsNullOrEmpty(options.Connection)
            || string.IsNullOrEmpty(options.TypeQueue)
            || string.IsNullOrEmpty(options.Message);
    }
}