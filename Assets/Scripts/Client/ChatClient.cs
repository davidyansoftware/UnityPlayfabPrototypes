using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class ChatClient : MonoBehaviour
{
    public ChatUI chatUI;

    public bool localClient;
    public string clientName;
    public int port = 6321;

    private bool connected = false;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConnectOnClick()
    {
        Connect("127.0.0.1", port);
        
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
