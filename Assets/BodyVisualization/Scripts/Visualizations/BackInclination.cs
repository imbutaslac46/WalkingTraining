using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class BackInclination : MonoBehaviour, IHideable
{
    public Text angleText;
    private CanvasGroup m_canvasGroup;

    private LineRenderer lineRenderer;

    private struct JointMeasure
    {
        public Kinect.JointType jointToMeasure;
    }

    private bool m_isVisible;

    private SkeletonVisualization m_skeletonVisualization;

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
            if (lineRenderer != null) lineRenderer.enabled = value;
            m_canvasGroup.alpha = value ? 1.0f : 0.0f;
        }
    }

    private void Awake()
    {
        JointMeasure jointMeasure = m_jointsToVisualize[0];

        m_canvasGroup = GetComponent<CanvasGroup>();
        m_skeletonVisualization = GameObject.FindObjectOfType<SkeletonVisualization>();
    }

    private void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.SetWidth(0.01f, 0.01f);
        lineRenderer.SetVertexCount(2);
        lineRenderer.enabled = false;

        angleText.text = "";
    }

    private void Update()
    {
        JointMeasure measure = m_jointsToVisualize[0];

        Vector3 spine, head;
        m_skeletonVisualization.GetJointWorldPosition(measure.jointToMeasure, out spine);
        m_skeletonVisualization.GetJointWorldPosition(Kinect.JointType.Head, out head);

        lineRenderer.SetPosition(0, spine);
        lineRenderer.SetPosition(1, head);

        float angle = Vector3.Angle((new Vector3(spine.x, spine.y, head.z) - spine).normalized, (head - spine).normalized);
        //Debug.Log("angle is" + angle);
        angleText.text = string.Format("{0}°", angle.ToString("f1"));
    }

}
