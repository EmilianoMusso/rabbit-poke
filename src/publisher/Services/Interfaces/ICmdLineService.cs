using poke.Models;
using System.Threading.Tasks;

namespace poke.Services.Interfaces;
public interface ICmdLineService
{
    CmdLineOptions Options { get; set; }
    Task Process();
}
