using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBody : MonoBehaviour {

    private TextMesh txtData;
    private double[] bodyData = new double[5]; //same size as storage

    void Start()
    {
        txtData = GameObject.Find("BodyData").GetComponent<TextMesh>();
    }

    void Update()
    {
        bodyData[0] = DataStore.Instance.smartex.storage[7]; //hr
        bodyData[1] = DataStore.Instance.smartex.storage[8]; //br

        object bTemp = DataStore.Instance.GetData("bTemp");
        if ( bTemp != null )
        {
            bodyData[2] = System.Convert.ToDouble(bTemp);
        }
        
        object bPs = DataStore.Instance.GetData("bPs");
        object bPd = DataStore.Instance.GetData("bPd");
        if ( bPs != null && bPd != null )
        {
            bodyData[3] = System.Convert.ToInt32(bPs);
            bodyData[4] = System.Convert.ToInt32(bPd);
        }

        txtData.text = System.String.Format("{0}\n\n\n" + "{1}\n\n\n" + "{2:F2}°C\n\n\n" + "{3}/{4}",
                                            bodyData[0], bodyData[1], bodyData[2], bodyData[3], bodyData[4]);
    }

    ////no need anymore but abstractvisualization wants this method
    //public override void UpdateProperty(string propertyName, object value)
    //{
    //    switch (propertyName)
    //    {
    //        case "hr":
    //            bodyData[0] = System.Convert.ToDouble(value);
    //            break;

    //        case "br":
    //            bodyData[1] = System.Convert.ToDouble(value);
    //            break;

    //        case "Temperature":
    //            bodyData[2] = System.Convert.ToDouble(value);
    //            break;

    //        case "Pressure":
    //            bodyData[3] = System.Convert.ToDouble(value);
    //            break;
    //    }

    //    UpdateText();

    //}

    ////Fice
    //private void UpdateText()
    //{
    //    txtData.text = System.String.Format("{0}\n\n\n" + "{1}\n\n\n" + "{2}\n\n\n" + "{3}",
    //                                        bodyData[0], bodyData[1], bodyData[2], bodyData[3]);
    //}


}

