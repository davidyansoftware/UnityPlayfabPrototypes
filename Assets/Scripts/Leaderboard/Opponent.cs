using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opponent : MonoBehaviour
{
    public Player player;

    public Text opponentName;

    public Text rating;
    public Text power;

    private string playfabId;

    public void SetPlayer(string playfabId, string opponentName)
    {
        this.playfabId = playfabId;
        this.opponentName.text = opponentName;
    }

    public void SetRatingPower(int rating, int power)
    {
        this.rating.text = rating.ToString();
        this.power.text = power.ToString();
    }

    public void OnClick()
    {
        Battle(player.PlayfabId, this.playfabId);
    }

    private void Battle(string playerId, string opponentId)
    {
        Debug.Log("PlayerId: " + playerId + "\nOpponentId: " + opponentId);

        // fetch stats of both players

        // simulate battle

        // increment decrement rating of both

        // return result - rating change?
    }
}
