
using BuildData;

public class GameManager
{
    // Creating the static array of 42 Territory object spaces
    public Territory[] GameTerritories { get; private set; } = new Territory[42];

    public void BuildGameMap()
    {
        var mapData = TerritoryDeserializer.LoadGameMap("Territories.json");
        Console.WriteLine("Map file loaded successfully!");
        // 1 -> Creates 42 Territory objects and stores them in the GameTerritories array
        // 2 -> Accesses each Territory object in the array and through a linked list, cretes adjacent Territory objects

        // Pass 1
        Console.WriteLine("Building Game objects...");
        foreach (var data in mapData.territories!)
        {
            GameTerritories[data.TerritoryID] = new Territory(data.TerritoryID, data.ContinentID, data.TerritoryName!);
        }

        // Pass 2
        foreach (var data in mapData.territories!)
        {
            var current = GameTerritories[data.TerritoryID];
            foreach (int adjacentID in data.AdjacentIDs!)
            {
                current.Adjacents.AddBack(GameTerritories[adjacentID]);
            }
        }

    }
}

class Program
{
    static void Main()
    {
        GameManager mainGame = new GameManager();
        mainGame.BuildGameMap();

        foreach (var territory in mainGame.GameTerritories)
        {
            Console.WriteLine($"{territory.TerritoryName} is adjacent to: ");
            foreach (var adjacent in territory.Adjacents)
            {
                Console.WriteLine(" - " + adjacent.TerritoryName);
            }
        }
    }
}