
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Client
{
    private Player? player = null;
    public async Task BuildClientAsync()
    {
        string serverIp = "192.168.100.7";
        int portNumber = 5000;
        int timeoutMilliseconds = 5000;

        using TcpClient client = new();

        try
        {
            // Setting connection timeout
            var connectTask = client.ConnectAsync(serverIp, portNumber);
            if (await Task.WhenAny(connectTask, Task.Delay(timeoutMilliseconds)) != connectTask)
            {
                throw new TimeoutException("Connection attempt timed out.");
            }

            Console.WriteLine("Connected to Game Server.");

            // Read and Write timeouts for the network stream
            client.ReceiveTimeout = timeoutMilliseconds;
            client.SendTimeout = timeoutMilliseconds;
            await using NetworkStream stream = client.GetStream();

            _ = ReceiveMessageAsync(stream);

            await HandleUserInputAsync(stream);

        }
        catch (TimeoutException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task SendMessageAsync(NetworkStream stream, byte[] message)
    {
        try
        {
            await stream.WriteAsync(message, 0, message.Length);
            Console.WriteLine("Message sent.");
        }
        catch (IOException ex) when (ex.InnerException is SocketException { SocketErrorCode: SocketError.TimedOut })
        {
            Console.WriteLine("Send operation timed out.");
        }
    }

    static async Task<int> ReceiveMessageAsync(NetworkStream stream)
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    PrintIncomingMessage(message);
                }
            }
        }
        catch (IOException ex) when (ex.InnerException is SocketException { SocketErrorCode: SocketError.TimedOut })
        {
            Console.WriteLine("Receive operation timed out.");
            return 0;
        }
    }

    private async Task HandleUserInputAsync(NetworkStream stream)
    {
        StringBuilder currentInput = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                string messageToSend = currentInput.ToString();
                if (!string.IsNullOrWhiteSpace(messageToSend))
                {
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    await stream.WriteAsync(data, 0, data.Length);
                }
                currentInput.Clear();
                Console.WriteLine();
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (currentInput.Length > 0)
                {
                    currentInput.Length--;
                    Console.Write("\b \b");
                }
            }
            else
            {
                currentInput.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
    }

    private static void PrintIncomingMessage(string message)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.WriteLine($"\n[Received] {message}");
        Console.Write("> ");
    }
}

/* public class Program
{
    public static async Task Main()
    {
        Client client1 = new Client();
        await client1.BuildClientAsync();
    } 
} */