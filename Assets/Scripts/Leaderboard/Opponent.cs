using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.CloudScriptModels;

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
        CallTestBattle(this.playfabId);
    }

    private void CallTestBattle(string playfabId)
    {
        ExecuteFunctionRequest request = new ExecuteFunctionRequest()
        {
            Entity = new EntityKey()
            {
                Id = PlayFabSettings.staticPlayer.EntityId, //Get this from when you logged in,
                Type = PlayFabSettings.staticPlayer.EntityType, //Get this from when you logged in
            },
            FunctionName = "TestBattle", //This should be the name of your Azure Function that you created.
            FunctionParameter = new Dictionary<string, object>() { { "opponent", playfabId } }, //This is the data that you would want to pass into your function.
            GeneratePlayStreamEvent = false //Set this to true if you would like this call to show up in PlayStream
        };
        PlayFabCloudScriptAPI.ExecuteFunction(
            request,
            (ExecuteFunctionResult result) => {
                if (result.FunctionResultTooLarge ?? false)
                {
                    Debug.Log("This can happen if you exceed the limit that can be returned from an Azure Function, See PlayFab Limits Page for details.");
                    return;
                }
                Debug.Log($"The {result.FunctionName} function took {result.ExecutionTimeMilliseconds} to complete");
                Debug.Log($"Result: {result.FunctionResult.ToString()}");
            },
            (PlayFabError error) => {
                Debug.Log($"Opps Something went wrong: {error.GenerateErrorReport()}");
            }
        );
    }
}