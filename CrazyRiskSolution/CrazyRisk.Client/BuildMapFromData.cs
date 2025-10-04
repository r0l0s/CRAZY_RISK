
using System.Reflection;
using System.Text.Json;

namespace BuildData;

// This class reflects the Json class structure
public class TerritoryData
{
    public int TerritoryID { get; set; }
    public int ContinentID { get; set; }
    public string? TerritoryName { get; set; }

    // First case of default List usage
    public List<int>? AdjacentIDs { get; set; }
}

// This class stores the deserialized TerritoryData objects of the Json file (42 in total)
public class MapData
{
    // Second and final use of a default List
    public List<TerritoryData>? territories { get; set; }
}

// This class reads the Json file and returns a MapData object representation of the file
public class TerritoryDeserializer
{
    public static MapData LoadGameMap(string fileName)
    {
        string jsonString;
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = $"CrazyRisk.Client.{fileName}";

        using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Could not find file '{fileName}' as external or embedded resource.");

        using StreamReader reader = new StreamReader(stream);
        jsonString = reader.ReadToEnd();

        return JsonSerializer.Deserialize<MapData>(jsonString!)!;
    }
    
    
}



/*
Each Json TerritoryData object has the following structure:

TerritoryID goes from 0 to 41
ContinentID goes from 0 to 5 (Asia=0, NorthAmerica=1, Europe=2, Africa=3, SouthAmerica=4, Oceania=5)

{
    "TerritoryID": id,
    "ContinentID": id,
    "TerritoryName": name,
    "AdjacentIDs": [ids]
}

*/