using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

using System.Collections.Generic;

public class SoleDataManager : MonoBehaviour
{
    public BaseClient baseClient;

    public int[] RightSole = new int[16];
    public int[] LeftSole = new int[16];

    void Update()
    {
    }

    private void OnEnable()
    {
        baseClient.RegisterTopicHandler("M2MQTT/RightSoleData", HandleRightSoleData);
        baseClient.RegisterTopicHandler("M2MQTT/LeftSoleData", HandleLeftSoleData);
    }

    private void OnDisable()
    {
        baseClient.UnregisterTopicHandler("M2MQTT/RightSoleData", HandleRightSoleData);
        baseClient.UnregisterTopicHandler("M2MQTT/LeftSoleData", HandleLeftSoleData);
    }

    private void HandleRightSoleData(string topic, string message)
    {
        JSONNode messageNode = JSON.Parse(message);
        JSONNode RightSoleData = messageNode["RightSoleData"];
        if (RightSoleData != null && RightSoleData.IsArray)
        {
            JSONArray RightSoleDataArray = RightSoleData.AsArray;
            for (int i = 0; i < 16; i++)
            {
                RightSole[i] = RightSoleDataArray[i].AsInt;
            }
        }
    }

    private void HandleLeftSoleData(string topic, string message)
    {
        JSONNode messageNode = JSON.Parse(message);
        JSONNode LeftSoleData = messageNode["LeftSoleData"];
        if (LeftSoleData != null && LeftSoleData.IsArray)
        {
            JSONArray LeftSoleDataArray = LeftSoleData.AsArray;
            for (int i = 0; i < 16; i++)
            {
                LeftSole[i] = LeftSoleDataArray[i].AsInt;
            }
        }
    }


}
