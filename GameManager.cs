
public class GameManager
{
    public List<Territory>? Territories { get; private set; }

    public void InitializeGame()
    {
        var mapData = MapLoader.LoadMap("Assets/Data/Territories.json");
        Territories = MapLoader.BuildTerritories(mapData);

        Console.WriteLine("Map loaded successfully!");
        Console.WriteLine($"Total Territories loaded: {Territories.Count}");
    }
}


class Program
{
    static void Main(string[] args)
    {
        GameManager mainGame = new GameManager();

        mainGame.InitializeGame();

        var TerritoryList = mainGame.Territories;

        var alaska = TerritoryList!.First(t => t.Id == 0);
        Console.WriteLine($"{alaska.Name} is adjacent to: ");

        foreach (var neighbor in alaska.Adjacents)
        {
            Console.WriteLine(" - " + neighbor.Name);
        }
    }


}