
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utils;
using Actions;

public class GameServer
{
    private readonly TcpListener listener;
    private int PlayerID = 0;

    private ClientProxy[] PlayersInGame = new ClientProxy[3];

    private GameQueue<IDataObject> distributionQueue = new GameQueue<IDataObject>();

    public GameServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task StartAsync()
    {
        ClientList.Add(0);
        ClientList.Add(1);
        ClientList.Add(2);
        
        listener.Start();
        Console.WriteLine("Server has started");

        while (true)
        {
            var TcpClient = await listener.AcceptTcpClientAsync();
            var proxy = new ClientProxy(TcpClient, this, PlayerID);
            Console.WriteLine("Client Proxy created.");
            PlayersInGame[PlayerID] = proxy;
            //ClientList.Add(proxy.playerID);
            PlayerID++;
            Console.WriteLine($"{PlayerID}");
            _ = proxy.ListenAsync();

        }
    }

    public void HandleIncomingDataObject(string json, ClientProxy prox)
    {
        var obj = DataObjectHandler.UnwrapObject(json);

        switch (obj)
        {
            case HandShake info:
                prox.Alias = info.Alias;
                Console.WriteLine($"Sending handshake confirmation to {prox.Alias}.");
                prox.Send(new HandShakeConfirmation
                {
                    Alias = prox.Alias,
                    PlayerID = prox.playerID

                }.WrapDataObject());



                if (PlayerID == 3)
                {
                    Thread.Sleep(2000);
                    var msg = Introduction();
                    foreach (var player in PlayersInGame)
                    {
                        player.Send(msg.WrapDataObject());
                    }
                    Thread.Sleep(2000);
                    Console.WriteLine("StartingDistribution");
                    GenerateDistribution();
                }
                
                break;
        }
    }

    public IDataObject Introduction()
    {
        var msg = new InitialUpdate
        {
            Player1ID = PlayersInGame[0].playerID,
            player1Alias = PlayersInGame[0].Alias,
            Player2ID = PlayersInGame[1].playerID,
            player2Alias = PlayersInGame[1].Alias,
            Player3ID = PlayersInGame[2].playerID,
            player3Alias = PlayersInGame[2].Alias
        };

        return msg;
    }


    public void GenerateDistribution()
    {
        Distributor generator = new Distributor(41);

        while (generator.HasTerritoryIDs)
        {   
            int PlayerID = ClientList.Traverse();
            int TerritoryID = generator.Select();
            
            
            var instruction = new TerritoryClaim
            {
                PlayerID = PlayerID,
                TerritoryID = TerritoryID
            };

            distributionQueue.Enqueue(instruction);

        }

        Console.WriteLine("Distribution Completed");
        Distribute();
    }

    public void Distribute()
    {
        for (int i = 0; i <= 41; i++)
        {
            var instruction = distributionQueue.Dequeue();
            foreach (var player in PlayersInGame)
            {
                player.Send(instruction.WrapDataObject());
                Thread.Sleep(100);
            }
        }
    }

}