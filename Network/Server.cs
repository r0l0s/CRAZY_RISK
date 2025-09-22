
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

public class Server
{
    private TcpListener listener;
    private ConcurrentDictionary<int, TcpClient> clients = new ConcurrentDictionary<int, TcpClient>();
    private int clientCounter = 0;

    public Server(int portNumber)
    {
        listener = new TcpListener(IPAddress.Any, portNumber);
    }

    public async Task StartServer()
    {
        listener.Start();
        Console.WriteLine("Server online... Waiting for clients.");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            int id = clientCounter++;

            if (clients.TryAdd(id, client))
            {
                Console.WriteLine($"Client {id} connected.");
                _ = ClientHandlerAsync(id, client);
            }
        }

        //Console.WriteLine("All 3 clients connected. Stopping listerner.");
        //listener.Stop();
    }

    private async Task ClientHandlerAsync(int id, TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnection 

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client {id} says: {message}");

                if (message.StartsWith("/to"))
                {
                    string[] parts = message.Split(' ', 3);
                    if (parts.Length == 3 && int.TryParse(parts[1], out int targetId))
                    {
                        await SendToClientAsync(targetId, $"[Private from {id}]: {parts[2]}");
                    }
                }
                else
                {
                    await BroadcastAsync($"Client {id}: {message}", id);
                }
            }
        }
        catch (Exception execption)
        {
            Console.WriteLine($"Error with client {id}: {execption.Message}");
        }
        finally
        {
            client.Close();
            clients.TryRemove(id, out _);
            Console.WriteLine($"Client {id} disconnected.");
        }
    }

    public async Task SendToClientAsync(int clientId, string message)
    {
        if (clients.TryGetValue(clientId, out TcpClient? client))
        {
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);
        }
    }

    public async Task BroadcastAsync(string message, int excludeId = -1)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (var kvp in clients)
        {
            if (kvp.Key == excludeId) continue;
            NetworkStream stream = kvp.Value.GetStream();
            await stream.WriteAsync(data, 0, data.Length);
        }
    }
}