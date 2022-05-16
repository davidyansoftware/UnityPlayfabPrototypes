using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class PlayfabFunctions
{
    public static void FetchPlayerStatistics(List<string> keys, Action<Dictionary<string, int>> handler)
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        request.StatisticNames = keys;
        PlayFabClientAPI.GetPlayerStatistics(
            request,
            (GetPlayerStatisticsResult result) =>
            {
                Dictionary<string, int> statistics = new Dictionary<string, int>();
                foreach (StatisticValue statistic in result.Statistics)
                {
                    statistics.Add(statistic.StatisticName, statistic.Value);
                }
                handler.Invoke(statistics);
            },
            OnNetworkError
        );
    }

    public static void FetchOpponentStatistic(string key, Action<int> handler)
    {
        GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
        request.StatisticName = key;
        request.MaxResultsCount = 1;
        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            request,
            (GetLeaderboardAroundPlayerResult opponentResult) =>
            {
                int statistic = opponentResult.Leaderboard[0].StatValue;
                handler(statistic);
            },
            OnNetworkError
        );
    }

    private static void OnNetworkError(PlayFabError response)
    {
        Debug.Log(response.ToString());
    }
}
