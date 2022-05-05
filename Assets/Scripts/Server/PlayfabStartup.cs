using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

public class PlayfabStartup : MonoBehaviour
{
    public int port = 6321;

    private ChatServer chatServer = new ChatServer();

    private void Start()
    {
        PlayFabMultiplayerAgentAPI.Start();
        PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;

        StartCoroutine(ReadyForPlayers());
    }

    IEnumerator ReadyForPlayers()
    {
        // not clear why delay is required
        yield return new WaitForSeconds(.5f);
        PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerActive()
    {
        chatServer.StartServer(port);
    }

    private void Update()
    {
        //TODO only tick every ~5 seconds

        chatServer.Tick();
    }

}
