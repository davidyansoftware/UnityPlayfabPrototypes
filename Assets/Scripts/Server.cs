using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.IO;
using System.Net;

public class Server : MonoBehaviour
{
    public int port = 6321;

    private TcpListener server;

    private List<ServerClient> clients = new List<ServerClient>();
    private List<ServerClient> disconnectedClients = new List<ServerClient>();

    private bool serverStarted = false;

    private void Start()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;

            Debug.Log("Server started on port: " + port.ToString());
        } catch (Exception e)
        {
            //TODO handle specific exceptions
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        //TODO only tick every ~5 seconds

        if (!serverStarted) return;

        foreach (ServerClient client in clients)
        {
            if (!IsConnected(client.tcpClient))
            {
                client.tcpClient.Close();
                disconnectedClients.Add(client);
                continue;
            }

            if (client.stream.DataAvailable)
            {
                string data = client.reader.ReadLine();

                if (data != null)
                {
                    OnIncomingData(client, data);
                }
            }
        }

        foreach(ServerClient disconnectedClient in disconnectedClients)
        {
            Broadcast(disconnectedClient.clientName + " has disconnected", clients);

            clients.Remove(disconnectedClient);
        }
        disconnectedClients.Clear();
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult result)
    {
        TcpListener listener = (TcpListener)result.AsyncState;
        ServerClient client = new ServerClient(listener.EndAcceptTcpClient(result));
        clients.Add(client);

        StartListening();

        Broadcast("%NAME", new List<ServerClient>() { client });
    }

    private bool IsConnected(TcpClient c)
    {
        if (c == null || c.Client == null || !c.Client.Connected)
        {
            return false;
        }

        try
        {
            if (c.Client.Poll(0, SelectMode.SelectRead))
            {
                int pingResponse = c.Client.Receive(new byte[1], SocketFlags.Peek);
                bool pingReceived = pingResponse != 0;

                return pingReceived;
            }

            return true;
        }
        catch
        {
            Debug.Log("Server: tcp error");
            return false;
        }
    }

    private void OnIncomingData(ServerClient client, string data)
    {
        if(data.Contains("&NAME")) {
            client.clientName = data.Split('|')[1];

            string connectionMessage = client.clientName + " has connected";
            Broadcast(connectionMessage, clients);

            return;
        }

        string message = client.clientName + ": " + data;
        Broadcast(message, clients);
    }

    private void Broadcast(string data, List<ServerClient> clients)
    {
        foreach (ServerClient client in clients)
        {
            try
            {
                client.writer.WriteLine(data);
                client.writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("write error: " + e.Message + " to client " + client.clientName);
            }
        }
    }
}

public class ServerClient
{
    public TcpClient tcpClient;
    public string clientName;

    public readonly NetworkStream stream;
    public readonly StreamReader reader;
    public readonly StreamWriter writer;

    public ServerClient(TcpClient tcpClient)
    {
        clientName = "GUEST";
        this.tcpClient = tcpClient;

        stream = tcpClient.GetStream();
        reader = new StreamReader(stream, true);
        writer = new StreamWriter(stream);
    }
}
