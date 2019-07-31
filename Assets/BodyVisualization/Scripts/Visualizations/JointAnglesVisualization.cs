using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class JointAnglesVisualization : MonoBehaviour, IHideable
{
    public GameObject textMeshPrefab;
    
    private struct JointMeasure
    {
        public Kinect.JointType jointToMeasure;

        public Kinect.JointType refJoint1;
        public Kinect.JointType refJoint2;
    }

    private bool m_isVisible;

    private SkeletonVisualization m_skeletonVisualization;
    private SkeletonManager m_skeletonManager;

    private Dictionary<Kinect.JointType, GameObject> m_jointAngleVisualizations;

    private List<JointMeasure> m_jointsToVisualize = new List<JointMeasure>()
    {
        new JointMeasure{ jointToMeasure = Kinect.JointType.ElbowLeft, refJoint1 = Kinect.JointType.ShoulderLeft, refJoint2 = Kinect.JointType.WristLeft },
        new JointMeasure{ jointToMeasure = Kinect.JointType.ElbowRight, refJoint1 = Kinect.JointType.ShoulderRight, refJoint2 = Kinect.JointType.WristRight },
        
        new JointMeasure{ jointToMeasure = Kinect.JointType.KneeLeft, refJoint1 = Kinect.JointType.HipLeft, refJoint2 = Kinect.JointType.AnkleLeft },
        new JointMeasure{ jointToMeasure = Kinect.JointType.KneeRight, refJoint1 = Kinect.JointType.HipRight, refJoint2 = Kinect.JointType.AnkleRight },
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

            foreach (GameObject go in m_jointAngleVisualizations.Values)
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
        m_jointAngleVisualizations = new Dictionary<Kinect.JointType, GameObject>();
        for (int i = 0; i < m_jointsToVisualize.Count; i++)
        {
            JointMeasure jointMeasure = m_jointsToVisualize[i];

            GameObject go = Instantiate(textMeshPrefab);
            go.name = jointMeasure.jointToMeasure.ToString() + "AngleTextMesh";
            go.transform.SetParent(this.transform, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            go.AddComponent<OffsetFromAnchorTowardsCamera>();
            go.AddComponent<HoloToolkit.Unity.Billboard>().PivotAxis = HoloToolkit.Unity.PivotAxis.Y;

            m_jointAngleVisualizations.Add(jointMeasure.jointToMeasure, go);
        }

        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        
        for (int i = 0; i < m_jointsToVisualize.Count; i++)
        {
            JointMeasure jointMeasure = m_jointsToVisualize[i];

            OffsetFromAnchorTowardsCamera temp = m_jointAngleVisualizations[jointMeasure.jointToMeasure].GetComponent<OffsetFromAnchorTowardsCamera>();
            temp.anchor = m_skeletonVisualization.GetJointGameObject(jointMeasure.jointToMeasure).transform;
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_jointsToVisualize.Count; i++)
        {
            JointMeasure measure = m_jointsToVisualize[i];

            Vector3 center, refJoint1, refJoint2;
            m_skeletonVisualization.GetJointWorldPosition(measure.jointToMeasure, out center);
            m_skeletonVisualization.GetJointWorldPosition(measure.refJoint1, out refJoint1);
            m_skeletonVisualization.GetJointWorldPosition(measure.refJoint2, out refJoint2);

            float angle = Vector3.Angle((refJoint1 - center).normalized, (refJoint2 - center).normalized);

            TextMesh textMesh = m_jointAngleVisualizations[measure.jointToMeasure].GetComponent<TextMesh>();
            textMesh.text = string.Format("{0}°", (int)angle);
            
            m_jointAngleVisualizations[measure.jointToMeasure].transform.position = m_skeletonVisualization.GetJointWorldPosition(measure.jointToMeasure);
            
            if (m_skeletonManager.ElbowLeftFlag == true && i == 0)
            {
                m_skeletonVisualization.SetBoneColorFromAngle(measure.jointToMeasure, angle);
                m_skeletonVisualization.SetBoneColorFromAngle(measure.refJoint2, angle);
            }

            else if (m_skeletonManager.ElbowRightFlag == true && i == 1)
            {
                m_skeletonVisualization.SetBoneColorFromAngle(measure.jointToMeasure, angle);
                m_skeletonVisualization.SetBoneColorFromAngle(measure.refJoint2, angle);
            }

            else if (m_skeletonManager.KneeLeftFlag == true && i == 2)
            {
                m_skeletonVisualization.SetBoneColorFromAngle(measure.jointToMeasure, angle);
                m_skeletonVisualization.SetBoneColorFromAngle(measure.refJoint2, angle);
            }

            else if (m_skeletonManager.KneeRightFlag == true && i == 3)
            {
                m_skeletonVisualization.SetBoneColorFromAngle(measure.jointToMeasure, angle);
                m_skeletonVisualization.SetBoneColorFromAngle(measure.refJoint2, angle);
            }
        }
    }

}
