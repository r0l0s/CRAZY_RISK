
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        GameServer gameserver = new GameServer(5000);

        await gameserver.StartAsync();
    } 
}