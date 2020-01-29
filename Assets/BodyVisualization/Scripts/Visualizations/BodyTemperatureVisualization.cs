using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyTemperatureVisualization : MonoBehaviour, IHideable
{
    public Text bodyTempText;
    private CanvasGroup m_canvasGroup;

    private SkeletonManager m_skeletonManager;

    public bool Visible
    {
        get
        {
            return m_canvasGroup.alpha > 0.0f;
        }

        set
        {
            m_canvasGroup.alpha = value ? 1.0f : 0.0f;
        }
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_skeletonManager = FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        bodyTempText.text = "25°C";
    }

    private void Update()
    {
        bodyTempText.text = m_skeletonManager.BodyTemp.ToString() + "°C";
    }
}
