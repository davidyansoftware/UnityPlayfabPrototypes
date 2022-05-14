using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text playerName;

    public Text rating;
    public Text power;

    private string playfabId;
    public string PlayfabId
    {
        get
        {
            return playfabId;
        }
    }

    public void SetPlayer(string playerName)
    {
        this.playerName.text = playerName;
    }

    public void SetRatingPower(int rating, int power)
    {
        this.rating.text = rating.ToString();
        this.power.text = power.ToString();
    }
}
