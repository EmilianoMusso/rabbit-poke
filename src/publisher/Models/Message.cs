namespace poke.Models;
public class Message
{
    public string HostName { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string ReplyTo { get; set; }
    public string Body { get; set; }

    public static Message FromOptions(CmdLineOptions options)
    {
        return new Message()
        {
            HostName = options.Connection,
            Exchange = options.Exchange,
            RoutingKey = options.TypeQueue,
            Body = options.Message,
            ReplyTo = options.ReplyTo
        };
    }
}