
using Actions;

public class LocalUser
{
    public string Alias { get; }
    private readonly Client client;

    private GameManager game;

    public event Action<string>? OnDisplayMessage;

    public LocalUser(string alias)
    {
        Alias = alias;
        client = new Client();
        game = new GameManager();
        client.OnMessageReceived += HandleIncomingDataObject;
        game.OnProcessCompletion += GameLayerState;
    }

    public async Task ConnectAsync(string ip, int port)
    {
        await client.ConnectAsync(ip, port, Alias);
        OnDisplayMessage?.Invoke($"LocalUser {Alias} connected.");
    }

    public void SendAction(IDataObject action)
    {
        client.Send(action.WrapDataObject());
    }


    public async Task BuildMapAsync()
    {
        await Task.Run(game.BuildGameMap);
    }

    private void GameLayerState(string json)
    {
        var obj = DataObjectHandler.UnwrapObject(json);

        switch (obj)
        {
            case ServerMessage message:
                OnDisplayMessage?.Invoke($"{message.Message}");
                break;
        }
    }


    // This my connection with the FrontEnd
    private void HandleIncomingDataObject(string json)
    {
        var obj = DataObjectHandler.UnwrapObject(json);

        switch (obj.Category)
        {
            /* case HandShakeConfirmation confirmation:
                OnDisplayMessage?.Invoke($"Game Server confirms Alias as: {confirmation.Alias}");
                OnDisplayMessage?.Invoke($"Game Server gave you the followng player ID: {confirmation.PlayerID}");
                break; */

            /* case ServerMessage message:
                OnDisplayMessage?.Invoke($"{message.Message}");
                break; */

            /* case InitialUpdate instruction:
                OnDisplayMessage?.Invoke($"ID => {instruction.Player1ID} with ALias => {instruction.player1Alias}");
                OnDisplayMessage?.Invoke($"ID => {instruction.Player2ID} with ALias => {instruction.player2Alias}");
                OnDisplayMessage?.Invoke($"ID => {instruction.Player3ID} with ALias => {instruction.player3Alias}");

                break; */

            case DataCategory.Claim:
                HandleClaimAction(obj);
                break;

        }
    }

    private void HandleClaimAction(IDataObject dataObject)
    {
        switch (dataObject)
        {
            case TerritoryClaim instruction:

                int PlayerID = instruction.PlayerID;
                int TerritoryID = instruction.TerritoryID;

                Player selected = game.GetPlayer(PlayerID);
                selected.Claim(TerritoryID);

                break;
        }
    }
}