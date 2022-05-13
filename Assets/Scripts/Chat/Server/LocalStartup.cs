using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalStartup : MonoBehaviour
{
    public int port = 6321;

    private ChatServer chatServer = new ChatServer();

    private void Start()
    {
        chatServer.StartServer(port);
    }

    private void Update()
    {
        //TODO only tick every ~5 seconds

        chatServer.Tick();
    }
}
