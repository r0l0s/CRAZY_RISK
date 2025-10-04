
using CrazyRisk.Shared.Data;
using CrazyRisk.Shared.Actions;
namespace CrazyRisk.Shared.Game;
public class GameManager
{
    // Creating the static array of 42 Territory object spaces
    public static Territory[] GameTerritories { get; private set; } = new Territory[42];

    // Players in the game
    public static Player[] PlayerList = [new Player(0), new Player(1), new Player(2)];

    public event Action<string>? OnProcessCompletion;

    

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

        var instruction = new ServerMessage { Message = "Map building completed" };

        foreach (var player in PlayerList)
            player.OnClaimed += OnProcessCompletion;

        OnProcessCompletion?.Invoke(instruction.WrapDataObject());

    }

    public Player GetPlayer(int ID)
    {
        return PlayerList[ID];
    }   
}   