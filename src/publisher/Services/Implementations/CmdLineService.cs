using poke.Models;
using poke.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace poke.Services.Implementations;
public class CmdLineService(string[] args) : ICmdLineService
{
    private readonly string[] _arguments = args;

    private readonly string[] _validOptions =
    [
        "--config",
        "--connection",
        "--type",
        "--message",
        "--exchange",
        "--replyto",
        "--wait"
    ];
    public CmdLineOptions Options { get; set; } = new();

    public async Task Process()
    {
        for (int i = 0; i < _arguments.Length; i += 2)
        {
            if (!_validOptions.Contains(_arguments[i]))
            {
                continue;
            }

            var optionValue = _arguments[i + 1];
            if (optionValue == null)
            {
                continue;
            }

            switch (_arguments[i].ToLower())
            {
                case "--config":
                    await LoadFromFile(optionValue);
                    break;

                case "--connection":
                    Options.Connection = optionValue;
                    break;

                case "--type":
                    Options.TypeQueue = optionValue;
                    break;

                case "--message":
                    Options.Message = optionValue;
                    break;

                case "--exchange":
                    Options.Exchange = optionValue;
                    break;

                case "--replyto":
                    Options.ReplyTo = optionValue;
                    break;

                case "--wait":
                    Options.WaitSeconds = int.Parse(optionValue);
                    break;
            }
        }
    }

    private async Task LoadFromFile(string filename)
    {
        var contents = await File.ReadAllTextAsync(filename);
        Options = JsonSerializer.Deserialize<CmdLineOptions>(contents);
    }
}