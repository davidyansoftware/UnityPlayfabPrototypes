using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public delegate void HandleTime(DateTime time);

public static class TimePlayfabFunctions
{
    public static void GetTime(HandleTime handler)
    {
        GetTimeRequest request = new GetTimeRequest(); //TODO just use same request
        PlayFabClientAPI.GetTime(
            request,
            (GetTimeResult result) => {
                handler(result.Time);
            },
            OnNetworkError
        );
    }

    private static void OnNetworkError(PlayFabError error)
    {
        Debug.LogError(error);
    }
}
