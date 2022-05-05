using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using PlayFab;
using PlayFab.MultiplayerModels;

public class ChatClient : MonoBehaviour
{
    public ChatUI chatUI;

    public bool localClient;
    public string clientName;
    public string buildId;
    public int port = 6321;

    private bool connected = false;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConnectOnClick()
    {
        if (localClient)
        {
            Connect("127.0.0.1", port);
        } else
        {
            RequestMultiplayerServer();
        }
    }

    private void RequestMultiplayerServer()
    {
        Debug.Log("[ClientStartUp].RequestMultiplayerServer");
        RequestMultiplayerServerRequest requestData = new RequestMultiplayerServerRequest();
        requestData.BuildId = buildId;
        requestData.SessionId = System.Guid.NewGuid().ToString();
        //TODO fix typeing error here
        //requestData.PreferredRegions = new List<AzureRegion>() { AzureRegion.EastUs };
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer, OnRequestMultiplayerServerError);
    }

    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        string host = response.IPV4Address;
        int port = (ushort)response.Ports[0].Num;

        Connect(host, port);
    }

    private void OnRequestMultiplayerServerError(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }

    private void Connect(string host, int port)
    {
        if (connected) return;

        try
        {
            //TODO do these need to be stored?
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            connected = true;

            Debug.Log("Client: connected to server");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        chatUI.OnSendMessage = Send;
    }

    private void Update()
    {
        if (!connected) return;

        if (stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
            {
                OnIncomingData(data);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
    private void OnDisable()
    {
        Disconnect();
    }

    private void OnIncomingData(string data)
    {
        //Debug.Log("Client: " + data);

        if (data == "%NAME")
        {
            Send("&NAME|" + clientName);

            return;
        }

        chatUI.AppendMessage(data);
    }

    private void Send(string data)
    {
        if (!connected) return;

        writer.WriteLine(data);
        writer.Flush();
    }

    private void Disconnect()
    {
        if (!connected) return;

        writer.Close();
        reader.Close();
        socket.Close();
        connected = false;
    }

}
