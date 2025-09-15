
public class Territory
{
    public int Id { get; private set; }
    public string Name { get; private set; }


    // Adjacent Territory objects
    public List<Territory> Adjacents { get; private set; } = new List<Territory>();


    // Game data
    public int Troops { get; set; } = 0;
    public Player? Owner { get; set; } = null;


    public Territory(int id, string name)
    {
        Id = id;
        Name = name;
    }
}