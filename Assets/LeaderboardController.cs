using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderboardController : MonoBehaviour
{
    private static readonly string RATING_KEY = "Rating";
    private static readonly string POWER_KEY = "Power";
    private static readonly List<string> DATA_KEYS = new List<string> { RATING_KEY, POWER_KEY }; 

    public Player player;

    public Opponent opponent1;
    public Opponent opponent2;
    public Opponent opponent3;

    private void Start()
    {
        PlayfabLogIn login = new PlayfabLogIn();
        login.AnonymousLogin(OnLogIn);
    }
    private void OnLogIn()
    {
        //TODO this should be handled on login and stored
        FetchDisplayName();

        FetchPlayer();

        FetchOpponents();
    }
    
    private void FetchDisplayName()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, DrawPlayerName, OnNetworkError);
    }
    private void DrawPlayerName(GetAccountInfoResult result)
    {
        string displayName = result.AccountInfo.TitleInfo.DisplayName;
        player.SetName(displayName);
    }

    private void FetchPlayer()
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        request.StatisticNames = DATA_KEYS;
        PlayFabClientAPI.GetPlayerStatistics(request, DrawPlayerRankPower, OnNetworkError);
    }
    private void DrawPlayerRankPower(GetPlayerStatisticsResult result)
    {
        //TODO find a better way to handle these defaults
        int rating = 1000;
        int power = 0;

        foreach(StatisticValue statistic in result.Statistics)
        {
            if (statistic.StatisticName.Equals(RATING_KEY))
            {
                rating = statistic.Value;
            }
            if (statistic.StatisticName.Equals(POWER_KEY))
            {
                power = statistic.Value;
            }
        }
        player.SetRatingPower(rating, power);
    }

    private void FetchOpponents()
    {
        //GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
        //request.StatisticName = RATING_KEY;
        //request.MaxResultsCount = 10;
    }
    private void OnFetchOpponentsSuccess(GetLeaderboardAroundPlayerResult result)
    {

    }

    private void FetchOpponent()
    {

    }

    private void OnNetworkError(PlayFabError error)
    {
        Debug.Log(error);
    }
}
