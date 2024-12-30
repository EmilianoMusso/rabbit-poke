namespace poke.Models;
public class CmdLineOptions
{
    public string Connection { get; set; }
    public string TypeQueue { get; set; }
    public string Message { get; set; }
    public string Exchange { get; set; }
    public string ReplyTo { get; set; }
    public int WaitSeconds { get; set; }
}