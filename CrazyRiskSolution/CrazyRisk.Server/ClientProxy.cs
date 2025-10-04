
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ClientProxy
{
    private readonly TcpClient tcpClient;
    private readonly GameServer server;
    private readonly NetworkStream stream;
    private readonly StreamWriter writer;
    public int playerID { get; private set; }

    public string Alias { get; set; } = "";
    

    public ClientProxy(TcpClient client, GameServer server, int ID)
    {
        playerID = ID;
        tcpClient = client;
        this.server = server;
        stream = tcpClient.GetStream();
        writer = new StreamWriter(stream, Encoding.UTF8) {AutoFlush = true};
    }

    public async Task ListenAsync()
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        while (true)
        {
            string? line = await reader.ReadLineAsync();
            if (line != null)
            {
                Console.WriteLine($"[Server] Received: {line}");
                server.HandleIncomingDataObject(line, this);
            }
        }
    }

    public void Send(string json)
    {
        writer.WriteLine(json);
        writer.Flush();
    }
}