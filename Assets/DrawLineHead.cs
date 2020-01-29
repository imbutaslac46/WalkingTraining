using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DrawLineHead : MonoBehaviour
{
    private LineRenderer lineRendererHead;
    public Material lineMaterial;
    public Material lineMaterial1;
    public Material lineMaterial2;
    //private GameObject HeightGuide;
    //private GameObject HeightGuide1;
    //private GameObject HeightGuide2;
    //private GameObject MarkerOrigin;

    //public float patientHeight = 1.75f;
    //float xDiff, yDiff, zRot;

    public Vector3 headOffset = Vector3.zero;

    private bool beingHandledHead = true;
    private bool isInitializedHead = false;
    private bool isFileClosedHead = true;
    private int arrayCounterHead;
    int i; //head counter

    private SkeletonVisualization m_skeletonVisualization;
    private SkeletonManager m_skeletonManager;

    private string headTrajectoryPath;
    TextWriter hd;

    List<Vector3> dataHD = new List<Vector3>();

    private void Awake()
    {
        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        lineRendererHead = GameObject.Find("LineRendererHead").GetComponent<LineRenderer>();
        lineRendererHead.SetWidth(0.01f, 0.01f);

        //MarkerOrigin = GameObject.Find("KinectMarkerOrigin");

        //HeightGuide = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //HeightGuide.name = "HeightGuide";
        //HeightGuide.transform.SetParent(MarkerOrigin.transform);
        //HeightGuide.transform.localPosition = Vector3.zero;
        //HeightGuide.transform.localScale = new Vector3(0.01f, 0.8f, 0.01f);
        //HeightGuide.GetComponent<Renderer>().sharedMaterial = lineMaterial;
        //Destroy(HeightGuide.GetComponent<Collider>());
        //HeightGuide.GetComponent<Renderer>().enabled = false;

        //HeightGuide1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //HeightGuide1.name = "HeightGuide1";
        //HeightGuide.transform.SetParent(MarkerOrigin.transform);
        //HeightGuide1.transform.localPosition = Vector3.zero;
        //HeightGuide1.transform.localScale = new Vector3(0.8f, 0.01f, 0.01f);
        //HeightGuide1.GetComponent<Renderer>().sharedMaterial = lineMaterial1;
        //Destroy(HeightGuide1.GetComponent<Collider>());
        //HeightGuide1.GetComponent<Renderer>().enabled = false;

        //HeightGuide2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //HeightGuide2.name = "HeightGuide2";
        //HeightGuide.transform.SetParent(MarkerOrigin.transform);
        //HeightGuide2.transform.localPosition = Vector3.zero;
        //HeightGuide2.transform.localScale = new Vector3(0.01f, 0.01f, 0.8f);
        //HeightGuide2.GetComponent<Renderer>().sharedMaterial = lineMaterial2;
        //Destroy(HeightGuide2.GetComponent<Collider>());
        //HeightGuide2.GetComponent<Renderer>().enabled = false;

        arrayCounterHead = 0;
    }

    private void Update()
    {
        //Debugging height
        /*
        Vector3 head = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head);
        Vector3 foot = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.FootLeft);
        float diff = head.z - foot.z;

        Debug.Log("height = " + diff);
        */

        if (m_skeletonManager.HeadTrajectoryFlag == true)
        {
            if (isInitializedHead == false)
            {
                lineRendererHead.SetVertexCount(3);

                lineRendererHead.SetPosition(0, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head) + headOffset);
                lineRendererHead.SetPosition(1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head) + headOffset);
                lineRendererHead.SetPosition(2, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head) + headOffset);

                i = 2;
                isInitializedHead = true;
                isFileClosedHead = false;

                //HeightGuide.GetComponent<Renderer>().enabled = true;
                //HeightGuide1.GetComponent<Renderer>().enabled = true;
                //HeightGuide2.GetComponent<Renderer>().enabled = true;
            }
            if (isInitializedHead == true && beingHandledHead == true)
            {
                //UpdateGuidePosition();
                StartCoroutine(HandleHead());
            }
        }

        if (m_skeletonManager.HeadTrajectoryFlag == false)
        {
            isInitializedHead = false;
            if (isFileClosedHead == false)
            {
                headTrajectoryPath = Path.Combine(Application.persistentDataPath, string.Format("Head{0}.txt", arrayCounterHead));
                hd = File.CreateText(headTrajectoryPath);

                Debug.Log("data created");
                foreach (var data in dataHD)
                {
                    hd.WriteLine(data.ToString("f5"));
                }
                hd.Dispose();

                arrayCounterHead++;
                isFileClosedHead = true;

                //HeightGuide.GetComponent<Renderer>().enabled = false;
                //HeightGuide1.GetComponent<Renderer>().enabled = false;
                //HeightGuide2.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private IEnumerator HandleHead()
    {
        i += 1;
        beingHandledHead = false;

        lineRendererHead.SetVertexCount(i);

        lineRendererHead.SetPosition(i - 1, m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head) + headOffset);

        dataHD.Add(m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head));

        yield return new WaitForSeconds(0.05f);
        beingHandledHead = true;
    }

    private void UpdateGuidePosition()
    {
        //Vector3 head = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.Head);
        //Vector3 shoulderLeft = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.ShoulderLeft);
        //Vector3 shoulderRight = m_skeletonVisualization.GetJointWorldPosition(Windows.Kinect.JointType.ShoulderRight);

        //HeightGuide.transform.position = new Vector3(head.x, head.y, head.z);
        //HeightGuide1.transform.position = new Vector3(head.x, head.y, head.z);
        //HeightGuide2.transform.position = new Vector3(head.x, head.y, head.z);

        //yDiff = shoulderLeft.normalized.y - shoulderRight.normalized.y;
        //xDiff = shoulderLeft.normalized.x - shoulderRight.normalized.x;
        //zRot = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;

        //HeightGuide.transform.eulerAngles = new Vector3(0, 0, zRot);
        //HeightGuide1.transform.eulerAngles = new Vector3(0, 0, zRot);
        //HeightGuide2.transform.eulerAngles = new Vector3(0, 0, zRot);
        //HeightGuide.transform.localRotation = Quaternion.Euler(0, 0, zRot); // red guideline rotation hololens not correct
    }

}
