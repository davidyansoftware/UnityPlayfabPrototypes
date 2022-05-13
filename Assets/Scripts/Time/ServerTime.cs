using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ServerTime : MonoBehaviour
{
    private const string FORMAT = "{0:00}:{1:00}:{2:00}";

    public Text serverTimeText;

    private DateTime serverTime;

    private bool timerStarted;

    // Start is called before the first frame update
    void Start()
    {
        PlayfabLogIn login = new PlayfabLogIn();
        login.AnonymousLogin(OnLogIn);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerStarted) return;

        serverTime = serverTime.AddSeconds(Time.deltaTime);

        serverTimeText.text = serverTime.ToString("yyyy/MM/dd HH:mm:ss");
    }

    private void OnLogIn()
    {
        TimePlayfabFunctions.GetTime(StartTimer);
    }

    private void StartTimer(DateTime serverTime)
    {
        this.serverTime = serverTime;
        timerStarted = true;
    }
}
