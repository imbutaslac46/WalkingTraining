using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

using System.Collections.Generic;

public class AppManager : MonoBehaviour
{
    public BaseClient baseClient;
    public ISkeletonProvider skeletonProvider;
    public SkeletonVisualization skeletonVisualization;
    public ObjManager objManager;
    public CalibrationManager calibrationManager;
    private GameObject heartRateVisualization;
    private DataVisualizationManager m_dataVisualizationManager;
    private GameObject m_jointAnglesVisualization;

    public enum AppState
    {
        Start, Running, CalibratingMarkerPosition, CalibratingSensorPosition
    }
    public AppState State
    {
        get;
        private set;
    }

    #region Public Functions

    public void StartMarkerPositionCalibration()
    {
        Debug.Log("Starting Marker Position Calibration");

        // Start calibration routine
        calibrationManager.CalibrateMarkerPosition();

        ChangeState(AppState.CalibratingMarkerPosition);
    }

    public void StopMarkerPositionCalibration()
    {
        Debug.Log("Stopping Marker Position Calibration");
        calibrationManager.StopCalibratingMarkerPosition();

        ChangeState(AppState.Running);
    }

    public void StartChairPositionCalibration()
    {
        Debug.Log("Starting Sensor Position Calibration");
        calibrationManager.CalibrateSensorsPositionOrigin();

        ChangeState(AppState.CalibratingSensorPosition);
    }

    public void StopChairPositionCalibration()
    {
        Debug.Log("Stopping Sensor Position Calibration");
        calibrationManager.StopCalibratingSensorsPositionOrigin();

        ChangeState(AppState.Running);
    }
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        m_dataVisualizationManager = new DataVisualizationManager();
    }

    private void Start()
    {
        State = AppState.Start;
        heartRateVisualization = objManager.SpawnFromDatabase("HeartRateVisualization", "HeartRateVisualization");
        heartRateVisualization.GetComponent<IHideable>().Visible = false;

        m_jointAnglesVisualization = objManager.SpawnFromDatabase("JointAnglesVisualization", "JointAnglesVisualization");
        m_jointAnglesVisualization.GetComponent<IHideable>().Visible = false;
    }

    private void Update()
    {
        UpdateSkeletonVisualization();
        m_dataVisualizationManager.OnUpdate();

        /*
        //keep track of the state of the HoloLens App
        if (State == AppState.Start)
        {
            // Do start stuff

            OffsetFromAnchorTowardsCamera temp = heartRateVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp != null)
            {
                temp.anchor = skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.SpineShoulder).transform;
            }

            ChangeState(AppState.Running);
        }

        else if (State == AppState.Running)
        {
        }
        */
    }

    private void OnEnable()
    {
        baseClient.RegisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/ChairData", HandleChairDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/SensorData", HandleSensorDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/Visualization", HandleVisualizationMqttMessage);
    }

    private void OnDisable()
    {
        baseClient.UnregisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/ChairData", HandleChairDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/SensorData", HandleSensorDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/Visualization", HandleVisualizationMqttMessage);
    }
    #endregion

    private void UpdateSkeletonVisualization()
    {
        if ( skeletonVisualization == null || skeletonProvider == null)
        {
            return;
        }

        Dictionary<Windows.Kinect.JointType, Vector3> jointPositions = skeletonProvider.GetJointPositions();
        skeletonVisualization.SetJointPositions(jointPositions);

        /*
        // Position the heart rate visualization on the spine shoulder joint
        Vector3 spineShoulderPos = skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineShoulder);
        Vector3 spineMidPos = skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid);
        heartRateVisualization.transform.position = (spineShoulderPos + spineMidPos) / 2;
        */
    }

    private void ChangeState(AppState newState)
    {
        if (State != newState)
        {
            HandleOnStateChange(State, newState);
            State = newState;
        }
    }

    private void HandleOnStateChange(AppState oldState, AppState newState)
    {
        // If ever there is a need to specially handle changing between certain scenes,
        // can do it here.
    }

    private void HandleMarkerPositionCalibrationFinished(Transform markerTransform)
    {
        ChangeState(AppState.Running);
    }

    //Logic for each WebApp method
    private void HandleWebAppMqttMessage( string topic, string message )
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

            if (flagName.Equals("CalibrateKinectMarker"))
            {
                if (flag)
                {
                    StartMarkerPositionCalibration();
                }
                else
                {
                    StopMarkerPositionCalibration();
                }
            }
            else if (flagName.Equals("CalibrateChairMarker"))
            {
                if (flag)
                {
                    StartChairPositionCalibration();
                }
                else
                {
                    StopChairPositionCalibration();
                }
            }
            else if (flagName.Equals("Skeleton"))
            {
                skeletonVisualization.Visible = flag;
            }
            else if (flagName.Equals("HeartSound"))
            {
                heartRateVisualization.GetComponent<HeartRateVisualization>().m_Play = flag;
            }
        }
        if (webAppNode["spawn"] != null)
        {
            JSONNode spawnNode = webAppNode["spawn"];
            JSONNode assetNameNode = spawnNode["object"];
            JSONNode gameObjectNameNode = spawnNode["name"];
            if (assetNameNode != null)
            {
                string assetName = assetNameNode.Value;
                string gameObjectName = assetName;
                if (gameObjectNameNode != null)
                {
                    gameObjectName = gameObjectNameNode.Value;
                }

                GameObject spawnedObject = objManager.SpawnFromDatabase(assetName, gameObjectName);
                if (spawnedObject != null)
                {
                    JSONNode originNode = spawnNode["parent"];
                    if (originNode != null)
                    {
                        string originName = originNode.Value;

                        GameObject originGameObject = GameObject.Find(originNode);
                        if (originGameObject != null)
                        {
                            spawnedObject.transform.SetParent(originGameObject.transform);
                        }
                    }

                    JSONNode locationNode = spawnNode["location"];
                    if (locationNode != null)
                    {
                        JSONArray arrayNode = locationNode.AsArray;

                        Vector3 location = new Vector3(arrayNode[0].AsFloat, arrayNode[1].AsFloat, arrayNode[2].AsFloat);
                        spawnedObject.transform.localPosition = location;
                    }
                }
            }
        }
        if (webAppNode["destroy"] != null)
        {
            JSONNode destroyNode = webAppNode["destroy"];
            JSONNode gameObjectNameNode = destroyNode["name"];
            if (gameObjectNameNode != null)
            {
                string gameObjectName = gameObjectNameNode.Value;
                objManager.Destroyz(gameObjectName);
            }
        }
        if (webAppNode["visible"] != null)
        {
            JSONNode visibleNode = webAppNode["visible"];

            JSONNode gameObjectNameNode = visibleNode["name"];
            string gameObjectName = gameObjectNameNode.Value;

            JSONNode valueNode = visibleNode["value"];
            bool value = valueNode.AsBool;

            GameObject go = objManager.GetInstantiatedObject(gameObjectName);
            if (go == null)
            {
                go = GameObject.Find(gameObjectName);
            }

            if(go != null)
            {
                // Check if the object has its own way of hiding itself.
                // Otherwise, just set the gameobject active flag
                IHideable hideable = go.GetComponent<IHideable>();
                if (hideable != null)
                {
                    hideable.Visible = value;
                }
                // Commented this out for now because when we set the gameobject to be inactive,
                // we can't find it again because GameObject.Find() does not return inactive objects.
                //else
                //{
                //    go.SetActive(value);
                //}
            }
        }
    }

    private void HandleSensorDataMqttMessage(string topic, string message)
    {
        JSONNode root = JSON.Parse(message);
        if (root != null)
        {
            JSONNode dataNode = root["data"];
            if ((dataNode != null) && dataNode.IsArray)
            {
                JSONNode dataArray = dataNode.AsArray;
                for (int i = 0; i < dataArray.Count; i++)
                {
                    JSONNode sensorDataNode = dataArray[i];

                    string sensorName = sensorDataNode["name"].Value;
                    double sensorValue = sensorDataNode["value"].AsDouble;
                    
                    DataStore.Instance.SetData(sensorName, sensorValue);
                }
            }
        }
    }

    private void HandleChairDataMqttMessage(string topic, string message)
    {
        Debug.Log(message);
        // For now, just destroy all visualizations when changing chair configurations.
        // Will think about a better way of dealing with this in the future
        List<string> chairSensors = DataStore.Instance.chairSensors;
        foreach( string sensorName in chairSensors )
        {
            AbstractVisualization visualization = m_dataVisualizationManager.GetDataVisualization(sensorName);
            if (visualization != null)
            {
                objManager.Destroyz(visualization.gameObject.name);
                m_dataVisualizationManager.UnregisterDataName(sensorName, false);
            }
        }
        chairSensors.Clear();

        JSONNode chairNode = JSON.Parse(message);
        JSONNode sensorsNode = chairNode["sensors"];
        if ((sensorsNode != null) && sensorsNode.IsArray)
        {
            JSONArray chairSensorsArray = sensorsNode.AsArray;
            for( int i = 0; i < chairSensorsArray.Count; i++)
            {
                JSONNode sensorNode = chairSensorsArray[i];

                JSONNode sensorNameNode = sensorNode["name"];
                string sensorName = sensorNameNode.Value;

                JSONArray sensorLocationArray = sensorNode["location"].AsArray;
                Vector3 sensorPosition = new Vector3(sensorLocationArray[0].AsFloat, 
                                                     sensorLocationArray[1].AsFloat, 
                                                     sensorLocationArray[2].AsFloat);
                
                DataStore.Instance.chairSensors.Add(sensorName);

                m_dataVisualizationManager.RegisterDataName(sensorName);
                m_dataVisualizationManager.SetVisualizationPosition(sensorName, sensorPosition);
            }
        }
    }

    private void HandleVisualizationMqttMessage(string topic, string message)
    {
        Debug.Log(message);
        JSONNode root = JSON.Parse(message);
        if (root != null)
        {
            JSONNode visualizationsNode = root["visualizations"];
            if (visualizationsNode != null)
            {
                JSONArray visualizationsArray = visualizationsNode.AsArray;
                for (int i = 0; i < visualizationsArray.Count; i++)
                {
                    JSONNode visualizationNode = visualizationsArray[i];

                    JSONNode dataNameNode = visualizationNode["data"];
                    if (dataNameNode != null)
                    {
                        string dataName = dataNameNode.Value;
                        if (!m_dataVisualizationManager.IsDataNameRegistered(dataName))
                        {
                            continue;
                        }

                        JSONNode objectNameNode = visualizationNode["object"];
                        string assetName = objectNameNode.Value;

                        AbstractVisualization visualization = m_dataVisualizationManager.GetDataVisualization(dataName);
                        // Check if the visualization type is different.
                        // Since we embed the name of the visualization type in the gameobject itself,
                        // we can just check if the gameobject name contains the name of the visualization type
                        // If it does, then it's the same visualization type, and no need to update it.
                        if ((visualization == null) || (!visualization.name.Contains(assetName)))
                        {
                            string visualizationObjName = dataName + "(" + assetName + ")";
                            GameObject obj = objManager.SpawnFromDatabase(assetName, visualizationObjName);
                            if (obj != null)
                            {
                                if (visualization != null)
                                {
                                    objManager.Destroyz(visualization.gameObject.name);
                                    m_dataVisualizationManager.SetVisualization(dataName, null);
                                }

                                // Handle re-parenting first (if necessary) before dealing with positioning
                                JSONNode parentNode = visualizationNode["parent"];
                                if (parentNode != null)
                                {
                                    string parentGameObjectName = parentNode.Value;
                                    GameObject parentObj = GameObject.Find(parentGameObjectName);
                                    if (parentObj != null)
                                    {
                                        obj.transform.SetParent(parentObj.transform, false);
                                    }
                                }

                                Vector3 visualizationPosition;
                                m_dataVisualizationManager.GetVisualizationPosition(dataName, out visualizationPosition);

                                JSONNode positionNode = visualizationNode["location"];
                                if (positionNode != null)
                                {
                                    JSONArray positionArray = positionNode.AsArray;
                                    visualizationPosition.x = positionArray[0].AsFloat;
                                    visualizationPosition.y = positionArray[1].AsFloat;
                                    visualizationPosition.z = positionArray[2].AsFloat;

                                    m_dataVisualizationManager.SetVisualizationPosition(dataName, visualizationPosition);
                                }
                                
                                obj.transform.localPosition = visualizationPosition;

                                visualization = obj.GetComponent<AbstractVisualization>();
                                if (visualization != null)
                                {
                                    m_dataVisualizationManager.SetVisualization(dataName, visualization);

                                    JSONNode propertiesNode = visualizationNode["properties"];
                                    if (propertiesNode != null)
                                    {
                                        foreach(JSONNode key in propertiesNode.Keys)
                                        {
                                            JSONNode propertyNode = propertiesNode[key.Value];
                                            if (propertyNode.IsNumber)
                                            {
                                                visualization.UpdateProperty(key.Value, propertyNode.AsDouble);
                                            }
                                            else if (propertyNode.IsString)
                                            {
                                                visualization.UpdateProperty(key.Value, propertyNode.Value);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // The object we're trying to spawn is not in the database.
                                Debug.LogError("Attempting to spawn a visualization gameobject not registered in the object database");
                            }
                        }
                    }
                }
            }
        }
    }
}
