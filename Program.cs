namespace AzurLaneScanner;

public class Program
{
    public static async Task Main(string[] args)
    {
        AzurLaneStatusScanner scanner = new AzurLaneStatusScanner();

        await Task.Delay(-1);
    }
}

