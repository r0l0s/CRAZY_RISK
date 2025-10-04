
using System.Text;
using CrazyRisk.Shared.Utils;
using CrazyRisk.Shared.Data;
using CrazyRisk.Shared.Actions;
namespace CrazyRisk.Shared.Game;

public class Player
{
    public int playerID { get; set; }
    public string playerAlias = "";
    public string playerColor = "";
    public int troopPool = 35;
    public int totalPlayerTroopCount = 0;
    public GameLinkedList<Territory> playerTerritories = new GameLinkedList<Territory>();

    public event Action<string>? OnClaimed;

    public Player(int id)
    {
        playerID = id;
    }

    public ContinentBonus continentControl = new ContinentBonus();


    public void Claim(int territoryID)
    {
        playerTerritories.AddBack(GameManager.GameTerritories[territoryID]);
        var instruction = new ServerMessage { Message = $"{playerID} got {GameManager.GameTerritories[territoryID].TerritoryName}" };
        OnClaimed?.Invoke(instruction.WrapDataObject());
    }

    public string ListMine()
    {
        var sb = new StringBuilder();
        foreach (var territory in playerTerritories)
        {
            sb.AppendLine(territory.TerritoryName + ",");
        }

        return sb.ToString();
    }


    // Player's turn available actions

    public void ReceiveTroops()
    {
        // Logic to receive a specific number of troops every turn
        int totalTerritories = playerTerritories.Count;

        int bonus = continentControl.GetContinentBonus();

        troopPool = (totalTerritories / 3) + bonus;

    }

    public void DraftTroops(string territoryName, int draftAmount)
    {
        foreach (var territory in playerTerritories!)
        {
            if (territory.TerritoryName == territoryName)
            {
                territory.AddTroops(draftAmount);
            }
        }

        Console.WriteLine($"");
    }

}