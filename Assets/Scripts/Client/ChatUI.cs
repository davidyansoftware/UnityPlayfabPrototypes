using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChatUI : MonoBehaviour
{
    public GameObject chatContainer;
    public MessagePrefab messagePrefab;

    public InputField message;

    public Action<string> OnSendMessage;

    public void SendOnClick()
    {
        SendMessage();
    }

    // this is called on submit and deselect. make sure message is only sent on submit
    public void OnEndEdit()
    {
        if(Input.GetButtonDown("Submit"))
        {
            SendMessage();
        }
    }

    public void AppendMessage(string message)
    {
        MessagePrefab prefab = GameObject.Instantiate(messagePrefab, chatContainer.transform);
        prefab.Setup(message);
    }

    private void SendMessage()
    {
        string data = message.text;
        OnSendMessage(data);

        message.text = "";
    }

}
