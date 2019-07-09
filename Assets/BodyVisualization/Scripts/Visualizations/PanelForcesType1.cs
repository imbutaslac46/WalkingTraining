using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelForcesType1 : AbstractVisualization
{

    private TextMesh textRight;
    private TextMesh textLeft;

    private double[] forceData = new double[6];

    // Use this for initialization
    void Start()
    {
        textRight = GameObject.Find("RightData").GetComponent<TextMesh>();
        textLeft = GameObject.Find("LeftData").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherData();

        UpdateText();

    }


    public override void UpdateProperty(string propertyName, object value)
    {

    }

    
    private void UpdateText()
    {
        textRight.text = System.String.Format("{0:F1}\n\n\n\n" + "{1:F1}\n\n\n\n\n" + "{2:F1}\n\n\n",
                                             forceData[4], forceData[0], forceData[2]);

        textLeft.text = System.String.Format("{0:F1}\n\n\n\n" + "{1:F1}\n\n\n\n\n" + "{2:F1}\n\n\n",
                                            forceData[5], forceData[1], forceData[3]);
    }

    private void GatherData()
    {
        object sitfr = DataStore.Instance.GetData("sitfr");
        if (sitfr != null)
        {
            forceData[0] = System.Convert.ToDouble(sitfr);
        }

        object sitfl = DataStore.Instance.GetData("sitfl");
        if (sitfl != null)
        {
            forceData[1] = System.Convert.ToDouble(sitfl);
        }

        object footr = DataStore.Instance.GetData("footr");
        if (footr != null)
        {
            forceData[2] = System.Convert.ToDouble(footr);
        }

        object footl = DataStore.Instance.GetData("footl");
        if (footl != null)
        {
            forceData[3] = System.Convert.ToDouble(footl);
        }

        object armr = DataStore.Instance.GetData("armr");
        if (armr != null)
        {
            forceData[4] = System.Convert.ToDouble(armr);
        }

        object arml = DataStore.Instance.GetData("arml");
        if (arml != null)
        {
            forceData[5] = System.Convert.ToDouble(arml);
        }

    }





}

