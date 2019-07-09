using UnityEngine;
using UnityEngine.UI;

public class BarGraph3D : AbstractVisualization, IHideable
{
    public GameObject barObject;
    public Text valueText;

    public string units = "kg";

    public double min;
    public double max;

    public double value;

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
    {}

    private void Start()
    {
        if (barObject != null)
        {
            m_barHeight = barObject.transform.localScale.y;
        }
    }

    private void Update()
    {
        // Clamp value between min and max
        if (value < min)
            value = min;
        if (value > max)
            value = max;
        
        double percentage = (value - min) / max;

        if (percentage >= 0)
        {
            UpdateBarObjectScale((float)(m_barHeight * percentage));
            UpdateValueText(value);
            
            if(!m_isVisible)
            {
                Visible = true;
            }
        }
        else if (!showOnZero)
        {
            Visible = false;
        }
    }

    private void UpdateBarObjectScale(float percentage)
    {
        if( barObject != null )
        {
            // For now, assume vertical
            Vector3 scale = barObject.transform.localScale;
            scale.y = percentage * 3;

            barObject.transform.localScale = scale;
        }
    }

    private void UpdateValueText(double value)
    {
        if (valueText != null)
        {
            valueText.text = string.Format("{0:F1} {1}", value, units);
        }
    }

    private void SetBarObjectVisible(bool visible)
    {
        if(barObject != null)
        {
            barObject.SetActive(visible);
        }
    }

    private void SetValueTextVisible(bool visible)
    {
        if(valueText != null)
        {
            valueText.gameObject.SetActive(visible);
        }
    }

    public override void UpdateProperty(string propertyName, object value)
    {
        if(propertyName.ToLower().Equals("value"))
        {
            this.value = System.Convert.ToDouble(value);
        }
    }
}
