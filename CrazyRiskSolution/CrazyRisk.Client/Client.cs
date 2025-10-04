
using System.Net.Sockets;
using System.Text;
using Actions;

public class Client
{
    private TcpClient? client;
    private NetworkStream? stream;
    private StreamWriter? writer;


    public event Action<string>? OnMessageReceived;

    public async Task ConnectAsync(string serverIp, int portNumber, string alias, int timeoutMilliseconds = 5000)
    {
        client = new TcpClient();

        try
        {
            var connectionTask = client.ConnectAsync(serverIp, portNumber);
            if (await Task.WhenAny(connectionTask, Task.Delay(timeoutMilliseconds)) != connectionTask)
                throw new TimeoutException("Connection attempt timed out.");

            Console.WriteLine("Connected to game server.");
            stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.UTF8) {AutoFlush = true};

            // Immediate handshake
            var handShake = new HandShake { Alias = alias };
            Send(handShake.WrapDataObject());
            Console.WriteLine("Handshake Sent.");

            // Start listening for incomming messages
            _ = Task.Run(async () => await ReceiveAsync(stream));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client error: " + ex.Message);
        }
    }

    private async Task ReceiveAsync(NetworkStream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        try
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) break;
                OnMessageReceived?.Invoke(line);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Receive error: " + ex.Message);
        }
    }

    public void Send(string json)
    {
        writer?.WriteLine(json);
    }
}