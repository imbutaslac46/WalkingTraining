using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBehavior : MonoBehaviour, IHideable
{
    public Text bloodPressureText;
    public Text breathRateText;
    public Text heartRateText;
    public Text bodyTemperatureText;

    private CanvasGroup m_canvasGroup;

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
    }

    private void Update()
    {
        UpdateBloodPressureText();
        UpdateBreathRateText();
        UpdateHeartRateText();
        UpdateBodyTemperatureText();
    }

    private void UpdateBloodPressureText()
    {
        
        if (bloodPressureText != null)
        {
            object bPsObj = DataStore.Instance.GetData("bPs");
            object bPdObj = DataStore.Instance.GetData("bPd");

            int bPs = 0, bPd = 0;
            if (bPsObj != null)
            {
                bPs = System.Convert.ToInt32(bPsObj);
            }
            if (bPdObj != null)
            {
                bPd = System.Convert.ToInt32(bPdObj);
            }

            bloodPressureText.text = "Blood Pressure: " + bPs + "/" + bPd;
        }
        
    }

    private void UpdateBreathRateText()
    {
        
        if (breathRateText != null)
        {
            breathRateText.text = "Breath Rate: " + DataStore.Instance.smartex.storage[8];
        }
       
    }

    private void UpdateHeartRateText()
    {
        
        if (heartRateText != null)
        {
            heartRateText.text = "Heart Rate: " + DataStore.Instance.smartex.storage[7];
        }
        
    }

    private void UpdateBodyTemperatureText()
    {
        
        if (bodyTemperatureText != null)
        {
            object bTempObj = DataStore.Instance.GetData("bTemp");
            double bTemp = 0.0;

            if (bTempObj != null)
            {
                bTemp = System.Convert.ToDouble(bTempObj);
            }

            bodyTemperatureText.text = string.Format("Body Temperature: {0:F2}°C", bTemp );
        }
      
    }
}
