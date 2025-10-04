
// This class handles the continent bonus logic

class Continent
{
    public int _currentTerritoryCount = 0;

    public void AddToCount()
    {
        _currentTerritoryCount++;
    }

    public void RemoveFromCount()
    {
        _currentTerritoryCount--;
    }
}

public class ContinentBonus
{

    // Asia -> NorthAmerica -> Europe -> Africa -> SouthAmerica -> Oceania

    private Continent[] playerContinentValues = new Continent[6];

    private int[] bonusValues = [7, 3, 5, 3, 2, 2];

    private int[] amountOfTerritories = [12, 9, 7, 6, 4, 4];

    public void AddTerritory(int id)
    {
        playerContinentValues[id].AddToCount();
    }

    public void RemoveTerritory(int id)
    {
        playerContinentValues[id].RemoveFromCount();
    }

    public int GetContinentBonus()
    {
        int bonus = 0;

        for (int i = 0; i < 6; i++)
        {
            if (playerContinentValues[i]._currentTerritoryCount == amountOfTerritories[i])
            {
                bonus += bonusValues[i];
            }
        }

        return bonus;

    }
}