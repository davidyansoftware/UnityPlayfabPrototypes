using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabLogIn
{
    // randomly generated custom id for editor account
    private static string EDITOR_ACCOUNT_CUSTOM_ID = "627307E8-4824174-340F9CF8-2CC2CDCC-1C15CCB6";

    private Action successCallback;

    public void Login(Action successCallback)
    {
        Debug.Log("[ClientStartUp].LoginRemoteUser");

        //TODO dont store this locally, just dynamically create function
        this.successCallback = successCallback;

#if UNITY_ANDROID
        LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            AndroidDeviceId = GetDeviceId()
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginError);
#elif UNITY_IOS
        LoginWithIOSDeviceIDRequest request = new LoginWithIOSDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            DeviceId = GetDeviceId()
        };

        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginError);
#else
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = GetDeviceId()
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
#endif
    }

    private string GetDeviceId()
    {
#if UNITY_ANDROID
        return SystemInfo.deviceUniqueIdentifier;
        //TODO https://answers.unity.com/questions/529513/getting-android-id-in-unity.html
#elif UNITY_IOS
        return SystemInfo.deviceUniqueIdentifier;
#elif UNITY_STANDALONE_WIN
        return SystemInfo.deviceUniqueIdentifier;
#else
        return EDITOR_ACCOUNT_CUSTOM_ID;
#endif
    }

    private void OnLoginSuccess(LoginResult response)
    {
        Debug.Log(response.ToString());

        successCallback();
    }

    private void OnLoginError(PlayFabError response)
    {
        Debug.Log(response.ToString());
    }
}

