using UnityEngine;

using M2MqttUnity;

using SimpleJSON;

using System.Collections.Generic;

public class AppManager : MonoBehaviour
{
    public BaseClient baseClient;
   // public ISkeletonProvider skeletonProvider;
   // public SkeletonVisualization skeletonVisualization;
    public ObjManager objManager;
    public CalibrationManager calibrationManager;
    private GameObject heartRateVisualization;
    private GameObject bodyTemperatureVisualization;
    private GameObject emgDataVisualization;
    private GameObject rightSoleVisualization;
    private GameObject leftSoleVisualization;
    private GameObject sideRightSoleVisualization;
    private GameObject sideLeftSoleVisualization;
    private DataVisualizationManager m_dataVisualizationManager;
    private RoadSpriteToggle m_roadSpriteToggle;
    private RightSoleTransform m_rightSoleTransform;
    private LeftSoleTransform m_leftSoleTransform;
    private GameObject m_jointAnglesVisualization;
    private GameObject m_postureVisualization;
    private GameObject m_backInclination;
    private GameObject m_velocityVisualization;
    private SkeletonVisualization m_skeletonVisualization;
    

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
        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_roadSpriteToggle = GameObject.FindObjectOfType<RoadSpriteToggle>();
        
    }

    private void Start()
    {
        State = AppState.Start;
        
        m_jointAnglesVisualization = objManager.SpawnFromDatabase("JointAnglesVisualization", "JointAnglesVisualization");
        m_jointAnglesVisualization.GetComponent<IHideable>().Visible = false;

        m_postureVisualization = objManager.SpawnFromDatabase("PostureVisualization", "PostureVisualization");
        m_postureVisualization.GetComponent<IHideable>().Visible = false;

        m_backInclination = objManager.SpawnFromDatabase("BackInclination", "BackInclination");
        m_backInclination.GetComponent<IHideable>().Visible = false;

        m_velocityVisualization = objManager.SpawnFromDatabase("VelocityVisualization", "VelocityVisualization");
        m_velocityVisualization.GetComponent<IHideable>().Visible = false;

        heartRateVisualization = objManager.SpawnFromDatabase("HeartRateVisualization", "HeartRateVisualization");
        heartRateVisualization.GetComponent<IHideable>().Visible = false;

        bodyTemperatureVisualization = objManager.SpawnFromDatabase("BodyTemperatureVisualization", "BodyTemperatureVisualization");
        bodyTemperatureVisualization.GetComponent<IHideable>().Visible = false;

        emgDataVisualization = objManager.SpawnFromDatabase("EMGGraph", "EMGGraph");
        emgDataVisualization.GetComponent<IHideable>().Visible = false;

        rightSoleVisualization = objManager.SpawnFromDatabase("RightSoleVisualization", "RightSoleVisualization");
        rightSoleVisualization.GetComponent<IHideable>().Visible = false;

        leftSoleVisualization = objManager.SpawnFromDatabase("LeftSoleVisualization", "LeftSoleVisualization");
        leftSoleVisualization.GetComponent<IHideable>().Visible = false;

        sideRightSoleVisualization = objManager.SpawnFromDatabase("SideRightSoleVisualization", "SideRightSoleVisualization");
        sideRightSoleVisualization.GetComponent<IHideable>().Visible = true;

        sideLeftSoleVisualization = objManager.SpawnFromDatabase("SideLeftSoleVisualization", "SideLeftSoleVisualization");
        sideLeftSoleVisualization.GetComponent<IHideable>().Visible = true;

        m_rightSoleTransform = GameObject.FindObjectOfType<RightSoleTransform>();
        m_leftSoleTransform = GameObject.FindObjectOfType<LeftSoleTransform>();
        sideRightSoleVisualization.GetComponent<IHideable>().Visible = false;
        sideLeftSoleVisualization.GetComponent<IHideable>().Visible = false;
    }

    private void Update()
    {
        m_dataVisualizationManager.OnUpdate();

        
        //keep track of the state of the HoloLens App
        if (State == AppState.Start)
        {
            // Do start stuff

            OffsetFromAnchorTowardsCamera temp = heartRateVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp != null)
            {
                temp.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.SpineShoulder).transform;
            }

            OffsetFromAnchorTowardsCamera temp0 = emgDataVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp0 != null)
            {
                temp0.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.ElbowRight).transform;
            }

            OffsetFromAnchorTowardsCamera temp1 = bodyTemperatureVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp1 != null)
            {
                temp1.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.SpineShoulder).transform;
            }

            OffsetFromAnchorTowardsCamera temp2 = m_backInclination.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp2 != null)
            {
                temp2.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.SpineShoulder).transform;
            }

            OffsetFromAnchorTowardsCamera temp3 = m_velocityVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp3 != null)
            {
                temp3.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.Head).transform;
            }

            OffsetFromAnchorTowardsCamera temp4 = rightSoleVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp4 != null)
            {
                temp4.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.FootRight).transform;
            }

            OffsetFromAnchorTowardsCamera temp5 = leftSoleVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp5 != null)
            {
                temp5.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.FootLeft).transform;
            }

            OffsetFromAnchorTowardsCamera temp6 = sideRightSoleVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp6 != null)
            {
                temp6.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.FootRight).transform;
            }

            OffsetFromAnchorTowardsCamera temp7 = sideLeftSoleVisualization.GetComponent<OffsetFromAnchorTowardsCamera>();
            if (temp7 != null)
            {
                temp7.anchor = m_skeletonVisualization.GetJointGameObject(Windows.Kinect.JointType.FootLeft).transform;
            }

            ChangeState(AppState.Running);
        }

        else if (State == AppState.Running)
        {
            //heartRateVisualization.GetComponent<IHideable>().Visible = true;
            Vector3 spineShoulderPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineShoulder);
            Vector3 spineMidPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid);
            Vector3 headPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head);
            Vector3 elbowRightPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.ElbowRight);
            Vector3 ankleRightPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.AnkleRight);
            Vector3 ankleLeftPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.AnkleLeft);
            Vector3 footRightPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight);
            Vector3 footLeftPos = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft);

            Vector3 midPos = (spineShoulderPos + spineMidPos) / 2;

            Vector3 soleRightDir = new Vector3(footRightPos.x - ankleRightPos.x, 0.0f, footRightPos.z - ankleRightPos.z).normalized;
            Quaternion soleRightRot = Quaternion.LookRotation(soleRightDir);

            Vector3 soleLeftDir = new Vector3(footLeftPos.x - ankleLeftPos.x, 0.0f, footLeftPos.z - ankleLeftPos.z).normalized;
            Quaternion soleLeftRot = Quaternion.LookRotation(soleLeftDir);

            sideRightSoleVisualization.transform.position = ankleRightPos;
            sideRightSoleVisualization.transform.rotation = soleRightRot;
            sideLeftSoleVisualization.transform.position = ankleLeftPos;
            sideLeftSoleVisualization.transform.rotation = soleLeftRot;


            heartRateVisualization.transform.position = midPos;
            emgDataVisualization.transform.position = elbowRightPos;
            rightSoleVisualization.transform.position = ankleRightPos;
            leftSoleVisualization.transform.position = ankleLeftPos;


            //change later to side
            bodyTemperatureVisualization.transform.position = new Vector3(midPos.x, midPos.y, midPos.z + 0.5f);

            m_backInclination.transform.position = spineShoulderPos;
            m_velocityVisualization.transform.position = new Vector3(headPos.x, headPos.y, headPos.z + 0.2f);
        }
        
    }

    private void OnEnable()
    {
        baseClient.RegisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/ChairData", HandleChairDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/RoadData", HandleRoadDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/SoleData", HandleSoleDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/SensorData", HandleSensorDataMqttMessage);
        baseClient.RegisterTopicHandler("M2MQTT/Visualization", HandleVisualizationMqttMessage);
    }

    private void OnDisable()
    {
        baseClient.UnregisterTopicHandler("M2MQTT/WebApp", HandleWebAppMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/ChairData", HandleChairDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/RoadData", HandleRoadDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/SoleData", HandleSoleDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/SensorData", HandleSensorDataMqttMessage);
        baseClient.UnregisterTopicHandler("M2MQTT/Visualization", HandleVisualizationMqttMessage);
    }
    #endregion
    /* moved skeleton implementation to another script
    private void UpdateSkeletonVisualization()
    {
        if ( skeletonVisualization == null || skeletonProvider == null)
        {
            return;
        }

        Dictionary<Windows.Kinect.JointType, Vector3> jointPositions = skeletonProvider.GetJointPositions();
        skeletonVisualization.SetJointPositions(jointPositions);

        
        // Position the heart rate visualization on the spine shoulder joint
        Vector3 spineShoulderPos = skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineShoulder);
        Vector3 spineMidPos = skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid);
        heartRateVisualization.transform.position = (spineShoulderPos + spineMidPos) / 2;
        
    }
    */

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
            else if (flagName.Equals("HeartRateVisualization"))
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

    private void HandleRoadDataMqttMessage(string topic, string message)
    {
        Debug.Log(message);

        JSONNode messageNode = JSON.Parse(message);
        JSONNode roadNode = messageNode["road"];
        if (roadNode != null && roadNode.IsArray)
        {
            JSONArray roadNodeArray = roadNode.AsArray;
            JSONNode roadData = roadNodeArray[0];

            JSONArray positionArray = roadData["position"].AsArray;
            Vector3 roadPosition = new Vector3(positionArray[0].AsFloat,
                                               positionArray[1].AsFloat,
                                               positionArray[2].AsFloat);

            JSONArray rotationArray = roadData["rotation"].AsArray;
            Vector3 roadRotation = new Vector3(rotationArray[0].AsFloat,
                                               rotationArray[1].AsFloat,
                                               rotationArray[2].AsFloat);

            JSONArray scaleArray = roadData["scale"].AsArray;
            Vector3 roadScale = new Vector3(scaleArray[0].AsFloat,
                                            scaleArray[1].AsFloat,
                                            scaleArray[2].AsFloat);

            JSONArray heightArray = roadData["height"].AsArray;
            Vector3 heightPosition = new Vector3(heightArray[0].AsFloat,
                                                 heightArray[1].AsFloat,
                                                 heightArray[2].AsFloat);

            JSONArray headOffsetArray = roadData["headoffset"].AsArray;
            Vector3 headOffset = new Vector3(headOffsetArray[0].AsFloat,
                                                 headOffsetArray[1].AsFloat,
                                                 headOffsetArray[2].AsFloat);

            JSONArray alphaValue = roadData["alpha"].AsArray;
       

            //Debug.Log("alpha value" + alphaValue);
            //Debug.Log("array = " + positionArray);
            //Debug.Log("x = " + positionArray[0]);
            //Debug.Log("y = " + positionArray[1]);
            //Debug.Log("z = " + positionArray[2]);
            m_roadSpriteToggle.SetPosition(roadPosition);
            m_roadSpriteToggle.SetRotation(roadRotation);
            m_roadSpriteToggle.SetScale(roadScale);
            m_roadSpriteToggle.SetHeight(heightPosition);
            m_roadSpriteToggle.SetAlpha(alphaValue[0].AsFloat);
            m_roadSpriteToggle.SetHeadOffset(headOffset);
        }
    }

    private void HandleSoleDataMqttMessage(string topic, string message)
    {
        Debug.Log(message);

        JSONNode messageNode = JSON.Parse(message);
        JSONNode soleNode = messageNode["sole"];
        if (soleNode != null && soleNode.IsArray)
        {
            JSONArray soleNodeArray = soleNode.AsArray;
            JSONNode soleData = soleNodeArray[0];

            JSONArray positionArrayRight = soleData["positionRight"].AsArray;
            Vector3 solePositionRight = new Vector3(positionArrayRight[0].AsFloat,
                                               positionArrayRight[1].AsFloat,
                                               positionArrayRight[2].AsFloat);

            JSONArray rotationArrayRight = soleData["rotationRight"].AsArray;
            Vector3 soleRotationRight = new Vector3(rotationArrayRight[0].AsFloat,
                                               rotationArrayRight[1].AsFloat,
                                               rotationArrayRight[2].AsFloat);

            JSONArray scaleArrayRight = soleData["scaleRight"].AsArray;
            Vector3 soleScaleRight = new Vector3(scaleArrayRight[0].AsFloat,
                                            scaleArrayRight[1].AsFloat,
                                            scaleArrayRight[2].AsFloat);

            JSONArray positionArrayLeft = soleData["positionLeft"].AsArray;
            Vector3 solePositionLeft = new Vector3(positionArrayLeft[0].AsFloat,
                                               positionArrayLeft[1].AsFloat,
                                               positionArrayLeft[2].AsFloat);

            JSONArray rotationArrayLeft = soleData["rotationLeft"].AsArray;
            Vector3 soleRotationLeft = new Vector3(rotationArrayLeft[0].AsFloat,
                                               rotationArrayLeft[1].AsFloat,
                                               rotationArrayLeft[2].AsFloat);

            JSONArray scaleArrayLeft = soleData["scaleLeft"].AsArray;
            Vector3 soleScaleLeft = new Vector3(scaleArrayLeft[0].AsFloat,
                                            scaleArrayLeft[1].AsFloat,
                                            scaleArrayLeft[2].AsFloat);

            m_rightSoleTransform.SetPosition(solePositionRight);
            m_rightSoleTransform.SetRotation(soleRotationRight);
            m_rightSoleTransform.SetScale(soleScaleRight);
            m_leftSoleTransform.SetPosition(solePositionLeft);
            m_leftSoleTransform.SetRotation(soleRotationLeft);
            m_leftSoleTransform.SetScale(soleScaleLeft);
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
