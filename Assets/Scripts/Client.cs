using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;

public class Client : MonoBehaviour
{
    public GameObject chatContainer;
    public MessagePrefab messagePrefab;

    public string clientName;
    public InputField message;

    public string host = "127.0.0.1";
    public int port = 6321;

    private bool connected = false;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public void ConnectOnClick()
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

    public void SendOnClick()
    {
        string data = message.text;
        Send(data);
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

    private void OnIncomingData(string data)
    {
        //Debug.Log("Client: " + data);

        if (data == "%NAME")
        {
            Send("&NAME|" + clientName);

            return;
        }

        MessagePrefab message = GameObject.Instantiate(messagePrefab, chatContainer.transform);
        message.Setup(data);
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

    private void OnApplicationQuit()
    {
        Disconnect();
    }
    private void OnDisable()
    {
        Disconnect();
    }
}
