using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DrawLineFeet : MonoBehaviour {

    private LineRenderer lineRendererFootRight;
    private LineRenderer lineRendererFootLeft;

    private bool beingHandledFeet = true;
    private bool isInitializedFeet = false;
    private bool isFileClosedFeet = true;
    private int arrayCounterFeet;
    int i; //feet counter
    
    private SkeletonVisualization m_skeletonVisualization;
    private SkeletonManager m_skeletonManager;

    private string footRightTrajectoryPath;
    private string footLeftTrajectoryPath;
    TextWriter fr;
    TextWriter fl;

    List<Vector3> dataFR = new List<Vector3>();
    List<Vector3> dataFL = new List<Vector3>();

    private void Awake()
    {
        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start ()
    {
        lineRendererFootRight = GameObject.Find("LineRendererFootRight").GetComponent<LineRenderer>();
        lineRendererFootLeft = GameObject.Find("LineRendererFootLeft").GetComponent<LineRenderer>();
        lineRendererFootRight.SetWidth(0.01f,0.01f);
        lineRendererFootLeft.SetWidth(0.01f, 0.01f);
        arrayCounterFeet = 0;
    }
	
	private void Update ()
    {
        if (m_skeletonManager.FeetTrajectoriesFlag == true)
        {
            if (isInitializedFeet == false)
            {
                lineRendererFootRight.SetVertexCount(3);
                lineRendererFootLeft.SetVertexCount(3);

                lineRendererFootRight.SetPosition(0, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight));
                lineRendererFootRight.SetPosition(1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight));
                lineRendererFootRight.SetPosition(2, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight));

                lineRendererFootLeft.SetPosition(0, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft));
                lineRendererFootLeft.SetPosition(1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft));
                lineRendererFootLeft.SetPosition(2, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft));

                i = 2;
                isInitializedFeet = true;
                isFileClosedFeet = false;
            }
            if (isInitializedFeet == true && beingHandledFeet == true)
            {
                StartCoroutine(HandleFeet());
            }
        }

        if (m_skeletonManager.FeetTrajectoriesFlag == false)
        {
            isInitializedFeet = false;
            if (isFileClosedFeet == false)
            {
                footRightTrajectoryPath = Path.Combine(Application.persistentDataPath, string.Format("FootRight{0}.txt", arrayCounterFeet));
                footLeftTrajectoryPath = Path.Combine(Application.persistentDataPath, string.Format("FootLeft{0}.txt", arrayCounterFeet));
                fr = File.CreateText(footRightTrajectoryPath);
                fl = File.CreateText(footLeftTrajectoryPath);

                Debug.Log("data created");
                foreach (var data in dataFR)
                {
                    fr.WriteLine(data.ToString("f5"));
                }
                foreach (var data in dataFL)
                {
                    fl.WriteLine(data.ToString("f5"));
                }

                fr.Dispose();
                fl.Dispose();

                arrayCounterFeet++;
                isFileClosedFeet = true;
            }
        }
    }

    private IEnumerator HandleFeet()
    {
        i += 1;
        beingHandledFeet = false;

        lineRendererFootRight.SetVertexCount(i);
        lineRendererFootLeft.SetVertexCount(i);

        lineRendererFootRight.SetPosition(i - 1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight));
        lineRendererFootLeft.SetPosition(i - 1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft));

        dataFR.Add(m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootRight));
        dataFL.Add(m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft));

        yield return new WaitForSeconds(0.05f);
        beingHandledFeet = true;
    }

}
