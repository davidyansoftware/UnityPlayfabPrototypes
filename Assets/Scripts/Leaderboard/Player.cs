using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text rank;
    public Text opponentName;
    public Text power;

    public void Setup(int rank, string opponentName, int power)
    {
        this.rank.text = rank.ToString();
        this.opponentName.text = opponentName;
        this.power.text = power.ToString();
    }
}
