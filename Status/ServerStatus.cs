namespace AzurLaneScanner.Status;

public class ServerStatus
{
    public int id { get; set; }
    public string name { get; set; }
    public ServerState state { get; set; }
    public ServerFlag flag { get; set; }
    public int sort { get; set; }
}