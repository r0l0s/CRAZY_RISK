
using Utils;
public class Player
{
    public string? playerAlias = null;
    public string? playerColor = null;
    public int troopPool = 35;
    public int totalPlayerTroopCount = 0;
    public GameLinkedList<Territory> playerTerritories = new GameLinkedList<Territory>();

    public ContinentBonus continentControl = new ContinentBonus();


    // Player's turn available actions

    public void ReceiveTroops()
    {
        // Logic to receive a specific number of troops every turn
        int totalTerritories = playerTerritories.Count;

        int bonus = continentControl.GetContinentBonus();

        troopPool = (totalTerritories / 3) + bonus;


    }
    
}