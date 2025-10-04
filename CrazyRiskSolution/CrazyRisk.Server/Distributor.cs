
namespace CrazyRisk.Shared.Utils;

public class Distributor
{
    private GameArrayList<int> TerritoryIDs;
    private Random rng = new Random();
    

    public Distributor(int maxNumber)
    {
        TerritoryIDs = new GameArrayList<int>();
        for (int i = 0; i <= maxNumber; i++)
        {
            TerritoryIDs.Add(i);
        }
    }
    public bool HasTerritoryIDs => TerritoryIDs.Count > 0;
    public int Select()
    {
        if (TerritoryIDs.Count == 0)
            throw new InvalidOperationException("Ran out of IDs");

        // Gets random index => TerritoryID
        int givenID = rng.Next(TerritoryIDs.Count);
        int value = TerritoryIDs[givenID];
        TerritoryIDs.RemoveAt(givenID);
        return value;
    }
}