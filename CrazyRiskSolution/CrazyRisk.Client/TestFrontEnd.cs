using CrazyRisk.Shared.Networking;
using CrazyRisk.Shared.Game;
using CrazyRisk.Shared.Data;
public class TestFrontEnd
{
    //private LocalUser? user;

    public static async Task Main()
    {
        Console.Write("Enter your username; ");
        string alias = Console.ReadLine()!;

        LocalUser user = new LocalUser(alias);

        user.OnDisplayMessage += msg => Console.WriteLine(msg);
        await user.ConnectAsync("192.168.100.56", 5000);
        await user.BuildMapAsync();

        while (true)
        {
            Console.WriteLine("Enter command");
            string input = Console.ReadLine()!;

            Console.WriteLine($"{input}");
        }
    }
}