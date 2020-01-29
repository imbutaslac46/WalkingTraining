using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class VelocityCalculator : MonoBehaviour
{
    public float waitTime = 0.05f;

    private SkeletonVisualization m_skeletonVisualization;
    private SkeletonManager m_skeletonManager;

    private float x1, x2, y1, y2, dist, sqrX, sqrY, instVelocity, sumAllVelocity, aveVelocity;
    private float timeElapsed, timeStart, timeEnd;

    List<float> arrayVelocity = new List<float>();

    private bool enterFlag = true;
    private bool isCounting = false;

    private string velocityPath;
    TextWriter saveVelocity;

    public float CurrSpeed // in kph *3.6f
    {
        get { return instVelocity * 100.0f; }
    }
    public float AveSpeed // in cm per second
    {
        get { return aveVelocity * 100.0f; }
    }
    public float DeltaTime // in sec
    {
        get { return timeElapsed; }
    }

    private void Awake()
    {
        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start ()
    {
        x1 = 0; x2 = 0; y1 = 0; y2 = 0;
        sqrX = 0; sqrY = 0; dist = 0;
        instVelocity = 0;
        aveVelocity = 0;
        timeElapsed = 0f; timeStart = 0f; timeEnd = 0f;
    }
	
	private void Update ()
    {
        if (m_skeletonManager.VelocityFlag == true)
        {
            StartCoroutine(GetVelocity());
            //Debug.Log("instantaneous velocity = " + instVelocity);
            enterFlag = false;

            if (isCounting == false)
            {
                timeStart = Time.time;
                isCounting = true;
            }
        }

        if (m_skeletonManager.VelocityFlag == false && enterFlag == false)
        {
            GetAverageVelocity();
            timeEnd = Time.time;
            timeElapsed = timeEnd - timeStart;

            //Debug.Log("average velocity = " + aveVelocity);
            //Debug.Log("time Elapsed = " + timeElapsed);

            velocityPath = Path.Combine(Application.persistentDataPath, string.Format("Velocity.txt"));
            saveVelocity = File.CreateText(velocityPath);

            Debug.Log("data created");
            foreach (var data in arrayVelocity)
            {
                saveVelocity.WriteLine(data.ToString());
            }
            saveVelocity.WriteLine("average = " + aveVelocity);
            saveVelocity.WriteLine("time = " + timeElapsed);
            saveVelocity.Dispose();

            isCounting = false;
            enterFlag = true;
        }
    }

    private IEnumerator GetVelocity()
    {
        x1 = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid).x;
        y1 = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid).y;

        yield return new WaitForSeconds(waitTime);

        x2 = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid).x;
        y2 = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.SpineMid).y;

        sqrX = Mathf.Pow(x2 - x1, 2);
        sqrY = Mathf.Pow(y2 - y1, 2);
        dist = Mathf.Sqrt(sqrX + sqrY);

        instVelocity = dist / waitTime;
        arrayVelocity.Add(instVelocity);
    }

    private void GetAverageVelocity()
    {
        sumAllVelocity = 0;
        
        foreach (float item in arrayVelocity)
        {
            sumAllVelocity += item;
        }

        aveVelocity = sumAllVelocity / arrayVelocity.Count;
    }
}
