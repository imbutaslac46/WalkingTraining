using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using HoloToolkit.Unity;

using Vuforia;

public class CalibrationManager : MonoBehaviour
{
    public ImageTargetBehaviour marker;

    public GameObject markerPositionVisualization;
    public GameObject kinectMarkerOriginAnchor;
    public GameObject chairAnchor;

    private bool m_calibrateKinectMarkerPosition = false;
    private bool m_calibrateSensorPositionsOrigin = false;

    private bool m_finishCalibration = false;

    private WorldAnchorManager m_worldAnchorManager;

    private enum State
    {
        Start, WaitingForAnchor, SetupVuforia, Idle, CalibratingMarkerPosition, CalibratingSensorsPositionOrigin
    }
    private State m_currentState;

    public void CalibrateMarkerPosition()
    {
        if (m_currentState == State.Idle)
        {
            m_calibrateKinectMarkerPosition = true;
        }
        else if( m_currentState == State.WaitingForAnchor)
        {
            Debug.Log("Still waiting for anchor!");
        }
    }

    public void StopCalibratingMarkerPosition()
    {
        if (m_currentState == State.CalibratingMarkerPosition)
        {
            m_finishCalibration = true;
        }
    }

    public void CalibrateSensorsPositionOrigin()
    {
        if(m_currentState == State.Idle)
        {
            m_calibrateSensorPositionsOrigin = true;
        }
        else if (m_currentState == State.WaitingForAnchor)
        {
            Debug.Log("Still waiting for anchor!");
        }
    }

    public void StopCalibratingSensorsPositionOrigin()
    {
        if(m_currentState == State.CalibratingSensorsPositionOrigin)
        {
            m_finishCalibration = true;
        }
    }

    #region Unity callback functions
    private void Awake()
    {
        m_worldAnchorManager = GameObject.FindObjectOfType<WorldAnchorManager>();
    }

    private void Start()
    {
        m_currentState = State.Start;
    }

    private void Update()
    {
        if ( m_currentState == State.Start )
        {
            SetMarkerPositionVisualizationVisible(false);
            
            m_currentState = State.WaitingForAnchor;
        }
        else if( m_currentState == State.WaitingForAnchor )
        {
            #if UNITY_WSA
            if( m_worldAnchorManager.AnchorStore != null )
            {
                m_worldAnchorManager.AttachAnchor(kinectMarkerOriginAnchor);
                m_worldAnchorManager.AttachAnchor(chairAnchor);

                m_currentState = State.SetupVuforia;
            }
            #endif
        }
        else if( m_currentState == State.SetupVuforia )
        {
            if( Vuforia.VuforiaRuntime.Instance.InitializationState == VuforiaRuntime.InitState.INITIALIZED )
            {
                StopVuforiaCamera();

                m_currentState = State.Idle;
            }
        }
        else if( m_currentState == State.Idle )
        {
            if( m_calibrateKinectMarkerPosition )
            {
                m_calibrateKinectMarkerPosition = false;

                m_worldAnchorManager.RemoveAnchor(kinectMarkerOriginAnchor);
                StartVuforiaCamera();
                
                SetMarkerPositionVisualizationVisible(true);

                m_currentState = State.CalibratingMarkerPosition;
            }
            else if( m_calibrateSensorPositionsOrigin )
            {
                m_calibrateSensorPositionsOrigin = false;

                m_worldAnchorManager.RemoveAnchor(chairAnchor);
                StartVuforiaCamera();
                
                SetMarkerPositionVisualizationVisible(true);

                m_currentState = State.CalibratingSensorsPositionOrigin;
            }
        }
        else if( m_currentState == State.CalibratingMarkerPosition )
        {
            if (m_finishCalibration)
            {
                m_finishCalibration = false;

                kinectMarkerOriginAnchor.transform.position = marker.transform.position;
                kinectMarkerOriginAnchor.transform.rotation = marker.transform.rotation;
                m_worldAnchorManager.AttachAnchor(kinectMarkerOriginAnchor);

                StopVuforiaCamera();
                
                SetMarkerPositionVisualizationVisible(false);

                m_currentState = State.Idle;
            }
        }
        else if(m_currentState == State.CalibratingSensorsPositionOrigin)
        {
            if(m_finishCalibration)
            {
                m_finishCalibration = false;


                chairAnchor.transform.position = marker.transform.position;
                chairAnchor.transform.rotation = marker.transform.rotation;
                m_worldAnchorManager.AttachAnchor(chairAnchor);

                StopVuforiaCamera();
                
                SetMarkerPositionVisualizationVisible(false);

                m_currentState = State.Idle;
            }
        }
    }
#endregion
    
    private void StartVuforiaCamera()
    {
        if (!Vuforia.CameraDevice.Instance.IsActive())
        {
            Vuforia.CameraDevice.Instance.Start();
        }
    }

    private void StopVuforiaCamera()
    {
        if( Vuforia.CameraDevice.Instance.IsActive() )
        {
            Vuforia.CameraDevice.Instance.Stop();
        }
    }

    private void SetMarkerPositionVisualizationVisible( bool visible )
    {
        if( markerPositionVisualization != null )
        {
            markerPositionVisualization.SetActive(visible);
        }
    }
}
