using System.Linq;

using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

public class SmartexMqttHandler : MonoBehaviour
{
    private BaseClient m_baseClient;

    private const string ECG_KEY = "ecg";
    private const string ACC_KEY = "acc";
    private const string PIEZO_KEY = "piezo";
    private const string QUALITY_KEY = "quality";
    private const string HR_KEY = "hr";
    private const string BR_KEY = "br";
    
    private void Awake()
    {
        m_baseClient = GameObject.FindObjectOfType<BaseClient>();
    }

    private void OnEnable()
    {
        m_baseClient.RegisterTopicHandler("M2MQTT/Smartex", HandleMqttMessage);
    }

    private void OnDisable()
    {
        m_baseClient.UnregisterTopicHandler("M2MQTT/Smartex", HandleMqttMessage);
    }

    private void HandleMqttMessage(string topic, string message)
    {
        JSONNode smartexNode = JSON.Parse(message);
        Smartex smartex = DataStore.Instance.smartex;

        //Check on bigger "time" and take array number
        //take value with that number and save it to array
        float maxValue;
        int maxIndex;

        if (smartexNode[ECG_KEY] != null)
        {
            JSONNode ecgNode = smartexNode[ECG_KEY];
            JsonUtility.FromJsonOverwrite(ecgNode.ToString(), smartex.ecg);

            maxValue = smartex.ecg.time.Max();
            maxIndex = smartex.ecg.time.ToList().IndexOf(maxValue);
            smartex.storage[0] = smartex.ecg.value[maxIndex];
        }
        else if (smartexNode[ACC_KEY] != null)
        {
            JSONNode accNode = smartexNode[ACC_KEY];
            JsonUtility.FromJsonOverwrite(accNode.ToString(), smartex.acc);

            maxValue = smartex.acc.time.Max();
            maxIndex = smartex.acc.time.ToList().IndexOf(maxValue);
            smartex.storage[1] = smartex.acc.x[maxIndex];
            smartex.storage[2] = smartex.acc.y[maxIndex];
            smartex.storage[3] = smartex.acc.z[maxIndex];
        }
        else if (smartexNode[PIEZO_KEY] != null)
        {
            JSONNode piezoNode = smartexNode[PIEZO_KEY];
            JsonUtility.FromJsonOverwrite(piezoNode.ToString(), smartex.piezo);

            maxValue = smartex.piezo.time.Max();
            maxIndex = smartex.piezo.time.ToList().IndexOf(maxValue);
            smartex.storage[4] = smartex.piezo.value[maxIndex];
        }
        else if (smartexNode[QUALITY_KEY] != null)
        {
            JSONNode qualityNode = smartexNode[QUALITY_KEY];
            JsonUtility.FromJsonOverwrite(qualityNode.ToString(), smartex.quality);

            maxValue = smartex.quality.time.Max();
            maxIndex = smartex.quality.time.ToList().IndexOf(maxValue);
            smartex.storage[5] = smartex.quality.hr[maxIndex];
            try
            {
                smartex.storage[6] = smartex.quality.br[maxIndex];
            }
            catch
            {
                smartex.storage[6] = 0;
            }

        }
        else if (smartexNode[HR_KEY] != null)
        {
            JSONNode hrNode = smartexNode[HR_KEY];
            JsonUtility.FromJsonOverwrite(hrNode.ToString(), smartex.hr);

            maxValue = smartex.hr.time.Max();
            maxIndex = smartex.hr.time.ToList().IndexOf(maxValue);
            smartex.storage[7] = smartex.hr.value[maxIndex];
        }
        else if (smartexNode[BR_KEY] != null)
        {
            JSONNode brNode = smartexNode[BR_KEY];
            JsonUtility.FromJsonOverwrite(brNode.ToString(), smartex.br);

            maxValue = smartex.br.time.Max();
            maxIndex = smartex.br.time.ToList().IndexOf(maxValue);
            smartex.storage[8] = smartex.br.value[maxIndex];
        }
    }
}
