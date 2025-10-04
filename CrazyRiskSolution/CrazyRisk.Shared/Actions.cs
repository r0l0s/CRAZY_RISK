namespace Actions;

public class HandShake : BaseDataObject
{
    public override DataCategory Category => DataCategory.Control;
    public string Alias { get; set; } = "";
}

public class HandShakeConfirmation : BaseDataObject
{
    public override DataCategory Category => DataCategory.Control;

    public string Alias { get; set; } = "";
    public int PlayerID { get; set; }
}

public class ServerMessage : BaseDataObject
{
    public override DataCategory Category => DataCategory.Control;
    public string Message { get; set; } = "";
}

public class InitialUpdate : BaseDataObject
{
    public override DataCategory Category => DataCategory.Control;
    public int Player1ID { get; set; }
    public string player1Alias { get; set; } = "";

    public int Player2ID { get; set; }
    public string player2Alias { get; set; } = "";

    public int Player3ID { get; set; }
    public string player3Alias { get; set; } = "";

}


public class TerritoryClaim : BaseDataObject
{
    public override DataCategory Category => DataCategory.Claim;

    public int PlayerID { get; set; }
    public int TerritoryID { get; set; }
}