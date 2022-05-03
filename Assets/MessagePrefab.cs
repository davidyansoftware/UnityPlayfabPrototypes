using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePrefab : MonoBehaviour
{
    [SerializeField] private Text text;

    public void Setup(string message)
    {
        text.text = message;
    }
}
