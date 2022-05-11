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

    public List<Opponent> opponents;

    private PlayfabLogIn login = new PlayfabLogIn();

    private void Start()
    {
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
        GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
        request.StatisticName = RATING_KEY;
        request.MaxResultsCount = 5;
        /*
        request.ProfileConstraints = new PlayerProfileViewConstraints()
        {
            ShowStatistics = true
        };
        */
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnFetchOpponentsSuccess, OnNetworkError);
    }
    private void OnFetchOpponentsSuccess(GetLeaderboardAroundPlayerResult result)
    {
        // remove player from leaderboard
        List<PlayerLeaderboardEntry> leaderboard = result.Leaderboard;
        for (int i = leaderboard.Count - 1; i >= 0; i--)
        {
            
            PlayerLeaderboardEntry entry = leaderboard[i];
            if (entry.PlayFabId.Equals(login.PlayfabId)) {
                leaderboard.RemoveAt(i);
            }
        }

        // choose opponents
        List<PlayerLeaderboardEntry> opponents = new List<PlayerLeaderboardEntry>();
        opponents.Add(leaderboard[0]);
        opponents.Add(leaderboard[leaderboard.Count / 2]);
        opponents.Add(leaderboard[leaderboard.Count - 1]);
        
        // populate prefabs
        for (int i = 0; i < opponents.Count; i++)
        {
            PlayerLeaderboardEntry opponent = opponents[i];
            Opponent prefab = this.opponents[i];

            string displayName = opponent.DisplayName;
            int rating = opponent.StatValue;
            int power = 0; //TODO get power from leaderboard/data

            GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
            request.StatisticName = POWER_KEY;
            request.MaxResultsCount = 1;
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, (GetLeaderboardAroundPlayerResult opponentResult) =>
            {
                power = opponentResult.Leaderboard[0].StatValue;
                prefab.SetName(displayName);
                prefab.SetRatingPower(rating, power);
            },
            OnNetworkError);
        }
    }

    private void OnNetworkError(PlayFabError error)
    {
        Debug.LogError(error);
    }
}
