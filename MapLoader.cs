
using System.Text.Json;


public class MapLoader
{
    public static MapData LoadMap(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<MapData>(json)!;
    }


    public static List<Territory> BuildTerritories(MapData mapData)
    {
        var territoryLookup = new Dictionary<int, Territory>();

        // Creating the actual Territory objects
        foreach (var data in mapData.territories!)
        {
            var territory = new Territory(data.id, data.name!);
            territoryLookup[data.id] = territory;
        }


        // Linking adjacent Territory objects to each Territory object
        foreach (var data in mapData.territories)
        {
            Territory current = territoryLookup[data.id];

            foreach (int adjacentId in data.adjacentTerritories!)
            {
                current.Adjacents.Add(territoryLookup[adjacentId]);
            }
        }

        return territoryLookup.Values.ToList();
    }
}
