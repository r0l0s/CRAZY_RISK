using Utils;
public class Territory
{
    public int TerritoryID { get; private set; }
    public int ContinentID { get; private set; }
    public string TerritoryName { get; private set; }

    // Adjacent Territory objects
    public GameLinkedList<Territory> Adjacents { get; private set; } = new GameLinkedList<Territory>();


    // Game data
    public int Troops { get; set; } = 0;
    public Player? Owner { get; set; } = null;


    public Territory(int territoryID, int continentID, string territoryName)
    {
        TerritoryID = territoryID;
        ContinentID = continentID;
        TerritoryName = territoryName;
    }

    public int AddTroops(int amountToAdd)
    {
        Troops += amountToAdd;
        return Troops;
    }

    public int RemoveTroops(int amountToRemove)
    {
        Troops -= amountToRemove;
        return Troops;
    }
}