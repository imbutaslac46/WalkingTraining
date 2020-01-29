using UnityEngine;
using UnityEngine.UI;

public class EMGGraph : MonoBehaviour, IHideable
{
    public GameObject barObject;
    public Text valueText;

    private SkeletonManager m_skeletonManager;

    public string units = "";

    public double min;
    public double max;

    public double emgValue;

    public bool showOnZero = true;

    private float m_barHeight = 1.0f;

    private bool m_isVisible = true;
    public bool Visible
    {
        get
        {
            return m_isVisible;
        }

        set
        {
            m_isVisible = value;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(value);
            }
        }
    }

    private void Awake()
    {
        m_skeletonManager = FindObjectOfType<SkeletonManager>();
    }

    private void Start()
    {
        if (barObject != null)
        {
            m_barHeight = barObject.transform.localScale.y;
        }
    }

    private void Update()
    {
        emgValue = m_skeletonManager.EMGData;
        // Clamp value between min and max
        if (emgValue < min)
            emgValue = min;
        if (emgValue > max)
            emgValue = max;

        double percentage = (emgValue - min) / max;

        UpdateBarObjectScale((float)(m_barHeight * percentage));
        UpdateValueText(emgValue);

        //if (percentage >= 0)
        //{
        //    UpdateBarObjectScale((float)(m_barHeight * percentage));
        //    UpdateValueText(emgValue);

        //    if (!m_isVisible)
        //    {
        //        Visible = true;
        //    }
        //}
        //else if (!showOnZero)
        //{
        //    Visible = false;
        //}
    }

    private void UpdateBarObjectScale(float percentage)
    {
        if (barObject != null)
        {
            // For now, assume vertical
            Vector3 scale = barObject.transform.localScale;
            scale.y = percentage / 2;

            barObject.transform.localScale = scale;
        }
    }

    private void UpdateValueText(double value)
    {
        if (valueText != null)
        {
            valueText.text = string.Format("{0:F0} {1}", value, units);
        }
    }

}
