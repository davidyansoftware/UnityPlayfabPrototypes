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

    public void SetRatingPower(int rating, int power)
    {
        this.rating.text = rating.ToString();
        this.power.text = power.ToString();
    }

    public void OnClick()
    {

    }
}
