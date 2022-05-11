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

    public void SetName(string opponentName)
    {
        this.opponentName.text = opponentName;
    }

    public void SetRankPower(string rating, string power)
    {
        this.rating.text = rating;
        this.power.text = power;
    }

    public void OnClick()
    {

    }
}
