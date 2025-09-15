
public class MapData
{
    public List<TerritoryData>? territories { get; set; }

}

public class TerritoryData
{
    public int id { get; set; }
    public string? name { get; set; }
    public List<int>? adjacentTerritories { get; set; }
}