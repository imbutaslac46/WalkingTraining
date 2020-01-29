using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class PostureVisualization : MonoBehaviour, IHideable
{
    public Material jointMaterial;
    public Material lineMaterial;

    private struct JointMeasure
    {
        public Kinect.JointType jointToMeasure;
    }

    private bool m_isVisible;

    private SkeletonVisualization m_skeletonVisualization;
    private SkeletonManager m_skeletonManager;

    private Dictionary<Kinect.JointType, GameObject> m_centerGravity;
    private Dictionary<Kinect.JointType, GameObject> m_lineGravity;

    private List<JointMeasure> m_jointsToVisualize = new List<JointMeasure>()
    {
        new JointMeasure{ jointToMeasure = Kinect.JointType.SpineBase}
    };

    public bool Visible
    {
        get
        {
            return m_isVisible;
        }

        set
        {
            m_isVisible = value;

            foreach (GameObject go in m_centerGravity.Values)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = m_isVisible;
                }
            }
            foreach (GameObject go in m_lineGravity.Values)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = m_isVisible;
                }
            }
        }
    }

    private void Awake()
    {
        m_centerGravity = new Dictionary<Kinect.JointType, GameObject>();

        JointMeasure jointMeasure = m_jointsToVisualize[0];

        GameObject coGravity = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        coGravity.name = jointMeasure.jointToMeasure.ToString() + "CoG";
        coGravity.transform.SetParent(this.transform, false);
        coGravity.transform.localPosition = Vector3.zero;
        coGravity.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        coGravity.GetComponent<Renderer>().sharedMaterial = jointMaterial;
        Destroy(coGravity.GetComponent<Collider>());
        m_centerGravity.Add(jointMeasure.jointToMeasure, coGravity);


        m_lineGravity = new Dictionary<Kinect.JointType, GameObject>();

        GameObject lineGravity = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lineGravity.name = jointMeasure.jointToMeasure.ToString() + "LoG";
        lineGravity.transform.SetParent(this.transform, false);
        lineGravity.transform.localPosition = Vector3.zero;
        lineGravity.transform.localScale = new Vector3(0.01f, 0.8f, 0.01f);
        lineGravity.GetComponent<Renderer>().sharedMaterial = lineMaterial;
        Destroy(lineGravity.GetComponent<Collider>());
        m_lineGravity.Add(jointMeasure.jointToMeasure, lineGravity);


        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        JointMeasure measure = m_jointsToVisualize[0];

        Vector3 spine, head, neck, footLeft, footRight, foot, center;
        m_skeletonVisualization.GetJointWorldPosition(measure.jointToMeasure, out spine);
        m_skeletonVisualization.GetJointWorldPosition(Kinect.JointType.Head, out head);
        m_skeletonVisualization.GetJointWorldPosition(Kinect.JointType.Neck, out neck);
        m_skeletonVisualization.GetJointWorldPosition(Kinect.JointType.FootLeft, out footLeft);
        m_skeletonVisualization.GetJointWorldPosition(Kinect.JointType.FootRight, out footRight);

        if (footLeft.y >= footRight.y)
        {
            foot = footRight;
        }
        else
        {
            foot = footLeft;
        }

        center = new Vector3(spine.x, ((head.y + foot.y) * 0.56f ) + head.y - neck.y, neck.z);

        m_centerGravity[measure.jointToMeasure].transform.position = center;
        m_lineGravity[measure.jointToMeasure].transform.position = center;
    }

}
