using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

using System.Collections.Generic;

public class SkeletonManager : MonoBehaviour
{
    public BaseClient baseClient;
    public ISkeletonProvider skeletonProvider;
    public SkeletonVisualization skeletonVisualization;

    public bool ElbowLeftFlag;
    public bool ElbowRightFlag;
    public bool KneeLeftFlag;
    public bool KneeRightFlag;

    void Update()
    {
        UpdateSkeletonVisualization();
    }

    private void OnEnable()
    {
        baseClient.RegisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
    }

    private void OnDisable()
    {
        baseClient.UnregisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
    }

    private void UpdateSkeletonVisualization()
    {
        if (skeletonVisualization == null || skeletonProvider == null)
        {
            return;
        }

        Dictionary<Windows.Kinect.JointType, Vector3> jointPositions = skeletonProvider.GetJointPositions();
        skeletonVisualization.SetJointPositions(jointPositions);

    }

    private void HandleWebAppMqttMessage(string topic, string message)
    {
        Debug.Log(message);
        JSONNode webAppNode = JSON.Parse(message);
        if (webAppNode == null)
        {
            return;
        }

        if (webAppNode["flag"] != null)
        {
            JSONNode flagNode = webAppNode["flag"];
            string flagName = flagNode["name"].Value;
            bool flag = flagNode["value"].AsBool;

            if (flagName.Equals("Skeleton"))
            {
                skeletonVisualization.Visible = flag;
            }
            else if (flagName.Equals("JointElbowLeft"))
            {
                ElbowLeftFlag = flag;
            }
            else if (flagName.Equals("JointElbowRight"))
            {
                ElbowRightFlag = flag;
            }
            else if (flagName.Equals("JointKneeLeft"))
            {
                KneeLeftFlag = flag;
            }
            else if (flagName.Equals("JointKneeRight"))
            {
                KneeRightFlag = flag;
            }
        }
    }

}
