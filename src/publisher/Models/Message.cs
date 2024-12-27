namespace poke.Models;
public class Message
{
    public string HostName { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string ReplyTo { get; set; }
    public string Body { get; set; }
}
