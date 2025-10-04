
using System.Text;
using Utils;
public class Player
{
    public int playerID;
    public string playerAlias = "";
    public string playerColor = "";
    public int troopPool = 35;
    public int totalPlayerTroopCount = 0;
    public GameLinkedList<Territory> playerTerritories = new GameLinkedList<Territory>();

    public ContinentBonus continentControl = new ContinentBonus();


    public void Claim(int territoryID)
    {
        playerTerritories.AddBack(GameManager.GameTerritories[territoryID]);
        Console.WriteLine($"{playerID} got {GameManager.GameTerritories[territoryID].TerritoryName}");
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