using poke.Models;
using System.Threading.Tasks;

namespace poke.Services.Interfaces;
public interface IRabbitService
{
    Task PublishAsync(Message message);
}