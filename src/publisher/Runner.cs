using Microsoft.Extensions.Logging;
using poke.Models;
using poke.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace poke;
public class Runner
{
    private readonly ILogger<Runner> _logger;
    private readonly ICmdLineService _cmdLineService;
    private readonly IRabbitService _rabbitService;

    public Runner(
        ILogger<Runner> logger,
        ICmdLineService cmdLineService,
        IRabbitService rabbitService)
    {
        _logger = logger;
        _cmdLineService = cmdLineService;
        _rabbitService = rabbitService;
    }

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
        if (string.IsNullOrEmpty(options.Connection)
            || string.IsNullOrEmpty(options.TypeQueue)
            || string.IsNullOrEmpty(options.Message))
        {
            _logger.LogError("One or more mandatory arguments was not specified.\nCheck for the following parameters:\n--connection\n--type\n--message");
            return;
        }

        var message = new Message()
        {
            HostName = options.Connection,
            Exchange = options.Exchange,
            RoutingKey = options.TypeQueue,
            Body = options.Message,
            ReplyTo = options.ReplyTo
        };

        await _rabbitService.PublishAsync(message);
        _logger.LogInformation("[{TimeStamp}] Event '{TypeName}' fired on bus", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), options.TypeQueue);

        if (!string.IsNullOrEmpty(options.ReplyTo))
        {
            var _waitSeconds = options.WaitSeconds > 0 ? options.WaitSeconds : 15;
            _logger.LogInformation("Waiting response for {WaitSeconds} seconds", _waitSeconds);
            Thread.Sleep(_waitSeconds * 1000);
        }
    }
}
