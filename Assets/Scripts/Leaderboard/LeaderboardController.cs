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

    private void DrawDisplayName(string name)
    {
        player.SetPlayer(name);
    }

    private void Start()
    {
        login.WithDisplayId(DrawDisplayName);
        login.WithPlayerStatistics(DrawPlayerRankPower);
        login.AnonymousLogin(OnLogIn);
    }
    private void OnLogIn()
    {
        FetchOpponents();
    }

    private void DrawPlayerRankPower(Dictionary<string, int> statistics)
    {
        //TODO find a better way to handle these defaults. title data?
        int rating = statistics.ContainsKey(RATING_KEY) ? statistics[RATING_KEY] : 1000;
        int power = statistics.ContainsKey(POWER_KEY) ? statistics[POWER_KEY] : 0;

        player.SetRatingPower(rating, power);
    }

    private void FetchOpponents()
    {
        GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
        request.StatisticName = RATING_KEY;
        request.MaxResultsCount = 5;
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

            string playfabId = opponent.PlayFabId;
            string displayName = opponent.DisplayName;
            int rating = opponent.StatValue;
            int power = 0; //TODO get power from leaderboard/data

            GetLeaderboardAroundPlayerRequest request = new GetLeaderboardAroundPlayerRequest();
            request.StatisticName = POWER_KEY;
            request.MaxResultsCount = 1;
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, (GetLeaderboardAroundPlayerResult opponentResult) =>
            {
                power = opponentResult.Leaderboard[0].StatValue;
                prefab.SetPlayer(playfabId, displayName);
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
