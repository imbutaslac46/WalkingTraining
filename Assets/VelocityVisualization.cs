using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using UnityEngine.UI;

public class VelocityVisualization : MonoBehaviour, IHideable
{
    public Text velocityText;
    private CanvasGroup m_canvasGroup;

    private VelocityCalculator SpeedCalculator;
    private SkeletonManager m_skeletonManager;

    private float prevSpeed, currSpeed, toleranceSpeed;

    private float countTime;

    private bool m_isVisible;
    public bool Visible
    {
        get
        {
            return m_isVisible;
        }

        set
        {
            m_isVisible = value;
            m_canvasGroup.alpha = value ? 1.0f : 0.0f;
        }
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        SpeedCalculator = GameObject.Find("SpeedCalculator").GetComponent<VelocityCalculator>();
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        velocityText.text = "";
        prevSpeed = 0; currSpeed = 0; toleranceSpeed = 1.0f;
        countTime = 0.2f;
    }

    private void Update()
    {
        if (m_skeletonManager.VelocityFlag == true)
        {
            if (countTime > 0)
            {
                countTime -= Time.deltaTime;
            }

            else
            {
                currSpeed = SpeedCalculator.CurrSpeed;
                if (Mathf.Abs(currSpeed - prevSpeed) < toleranceSpeed)
                {
                    currSpeed = prevSpeed;
                }

                velocityText.text = string.Format("Velocity = {0} [cm/s]", currSpeed.ToString("f2"));
                prevSpeed = currSpeed;

                countTime = 0.2f;
                //Debug.Log("Time reset");
            }
        }
        if (m_skeletonManager.VelocityFlag == false)
        {
            velocityText.text = string.Format("Time = {0} [s] \n Velocity(ave) = {1} [cm/s]", SpeedCalculator.DeltaTime.ToString("f1"), SpeedCalculator.AveSpeed.ToString("f2"));
        }
    }

}
