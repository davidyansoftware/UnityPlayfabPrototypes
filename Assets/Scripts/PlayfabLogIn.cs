using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabLogIn
{
    private Action successCallback;

    private string playfabId;
    public string PlayfabId
    {
        get
        {
            return playfabId;
        }
    }

    //TODO use builder pattern to set these
    private GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
    private Action<string> displayNameHandler;
    private Action<Dictionary<string, int>> statisticsHandler;

    public void WithDisplayId(Action<string> handler)
    {
        infoRequestParams.GetPlayerProfile = true;
        displayNameHandler = handler;
    }

    public void WithPlayerStatistics(Action<Dictionary<string, int>> handler)
    {
        infoRequestParams.GetPlayerStatistics = true;
        statisticsHandler = handler;
    }

    private void HandleLoginData(LoginResult result)
    {
        Debug.Log(result.ToString());

        //TODO will be an error if account doesnt exist
        // noted in docs: GetPlayerProfile Has no effect for a new player
        // https://docs.microsoft.com/en-us/rest/api/playfab/client/account-management/get-player-combined-info?view=playfab-rest#getplayercombinedinforequestparams
        displayNameHandler?.Invoke(result.InfoResultPayload.PlayerProfile.DisplayName);

        if (statisticsHandler != null)
        {
            Dictionary<string, int> statistics = new Dictionary<string, int>();
            foreach(StatisticValue statistic in result.InfoResultPayload.PlayerStatistics)
            {
                statistics.Add(statistic.StatisticName, statistic.Value);
            }
            statisticsHandler.Invoke(statistics);
        }
    }

    public void AnonymousLogin(Action successCallback)
    {
        Debug.Log("[ClientStartUp].LoginRemoteUser");

        //TODO dont store this locally, just dynamically create function
        this.successCallback = successCallback;

#if UNITY_ANDROID
        LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            AndroidDeviceId = GetDeviceId(),
            InfoRequestParameters = infoRequestParams
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginError);
#elif UNITY_IOS
        LoginWithIOSDeviceIDRequest request = new LoginWithIOSDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            DeviceId = GetDeviceId(),
            InfoRequestParameters = infoRequestParams
        };

        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginError);
#else
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = GetDeviceId(),
            InfoRequestParameters = infoRequestParams
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
        HandleLoginData(response);

        //TODO make this static? would that cause issues?
        playfabId = response.PlayFabId;
        Debug.Log("PlayfabId: " + playfabId);

        successCallback();
    }

    private void OnLoginError(PlayFabError response)
    {
        Debug.Log(response.ToString());
    }

    //TODO
    // register account
    // login facebook
    // login google
    private void AddLogin(string displayName, string email, string password)
    {
        AddUsernamePasswordRequest request = new AddUsernamePasswordRequest { Username = displayName, Email = email, Password = password };
        PlayFabClientAPI.AddUsernamePassword(request, OnAddLoginSuccess, OnAddLoginError);
    }
    private void OnAddLoginSuccess(AddUsernamePasswordResult response)
    {
        //TODO store data locally
    }
    private void OnAddLoginError(PlayFabError response) {
        Debug.Log(response.ToString());
    }

}

