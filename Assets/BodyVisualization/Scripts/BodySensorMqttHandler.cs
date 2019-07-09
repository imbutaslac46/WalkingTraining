using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

public class BodySensorMqttHandler : MonoBehaviour
{
    private BaseClient m_baseClient;

    private void Awake()
    {
        m_baseClient = GameObject.FindObjectOfType<BaseClient>();
    }

    private void OnEnable()
    {
        m_baseClient.RegisterTopicHandler("M2MQTT/SensorData", HandleMqttMessage);
    }
    
    private void OnDisable()
    {
        m_baseClient.UnregisterTopicHandler("M2MQTT/SensorData", HandleMqttMessage);
    }

    private void HandleMqttMessage(string topic, string message)
    {
        JSONNode root = JSON.Parse(message);

        JSONNode bodyNode = root["body"];
        if (bodyNode != null)
        {
            JSONNode bTemp = bodyNode["bTemp"];
            DataStore.Instance.SetData("bTemp", bTemp.AsDouble);

            JSONNode bPs = bodyNode["bPs"];
            DataStore.Instance.SetData("bPs", bPs.AsDouble);

            JSONNode bPd = bodyNode["bPd"];
            DataStore.Instance.SetData("bPd", bPd.AsDouble);
        }
    }
}
