using System.Text.Json;
using AzurLaneScanner.Status;

namespace AzurLaneScanner;

public class AzurLaneStatusScanner
{
    private List<ServerStatus> _oldStatusList = new();
    private List<ServerStatus> _newStatusList = new();
    private HttpClient _client = new();

    public AzurLaneStatusScanner()
    {
        _client.BaseAddress = new Uri("http://blhxusgate.yo-star.com/?cmd=load_server?ALServer.json");
        Forever();
    }

    public async Task<List<ServerStatus>> GetStatus()
    {
        var res = await _client.GetAsync(String.Empty);

        if (!res.IsSuccessStatusCode)
            throw new Exception();

        var str = await res.Content.ReadAsStringAsync();
        var status = JsonSerializer.Deserialize<List<ServerStatus>>(str);

        if (status is null)
            throw new Exception();

        return status;
    }

    public async void Forever()
    {
        Console.WriteLine("Scanner Active");
        Console.Beep();
        
        _oldStatusList = await GetStatus();
        
        foreach (var server in _oldStatusList)
        {
            Console.WriteLine($"{server.name}: Status [{server.state}]");
        }
        
        while (true)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                
                bool stateChanged = false;
                _newStatusList = await GetStatus();

                foreach (var server in _newStatusList)
                {
                    if (server.state != _oldStatusList.Single(e => e.id == server.id).state)
                        stateChanged = true;
                }

                if (stateChanged)
                {
                    Console.WriteLine($"!!!!!!!! SERVER STATE CHANGED !!!!!!!!");
                    Console.Beep();

                    foreach (var server in _newStatusList)
                    {
                        Console.WriteLine($"{server.name}: Status [{server.state}]");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}