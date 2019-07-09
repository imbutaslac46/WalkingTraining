///https://www.youtube.com/watch?v=6yDRbnXve_0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

/// <summary>
/// Used to receive a Json string and save the data into the right sensor class
/// </summary>
public class JsonSaver : MonoBehaviour {

    public PowerSensor powerSensorBed;
    public Empatica empatica;
    public Smartex smartex = new Smartex();

    public static JsonSaver instanceJSON;


    void Awake()
    {
        if (instanceJSON == null)
        {
            instanceJSON = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void storeVars(string topic, string message)
    {

        if (topic.Contains("WebApp"))
        {
            return;
        }


        if (topic.Contains("powerSensor"))
        {
            if (topic.Contains("bed"))
            {
                message= message.Replace("[", "");
                message= message.Replace("]", "");
                JsonUtility.FromJsonOverwrite(message, powerSensorBed);
            }
        }


        if (topic.Contains("empatica"))
        {
            JsonUtility.FromJsonOverwrite(message, empatica);
            //Debug.Log(empatica.accelerations);
        }


        if (topic.Contains("smartex"))
        {
            //Check on bigger "time" and take array number
            //take value with that number and save it to array
            float maxValue;
            int maxIndex;


            if (message.Contains("ecg"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length-2); //removes last }
                message = message.Remove(0,11); //removes {"ecg": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.ecg);

                maxValue = smartex.ecg.time.Max();
                maxIndex = smartex.ecg.time.ToList().IndexOf(maxValue);
                smartex.storage[0] = smartex.ecg.value[maxIndex];
            }
            else if (message.Contains("acc"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length - 1); //removes last }
                message = message.Remove(0, 11); //removes {"acc": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.acc);

                maxValue = smartex.acc.time.Max();
                maxIndex = smartex.acc.time.ToList().IndexOf(maxValue);
                smartex.storage[1] = smartex.acc.x[maxIndex];
                smartex.storage[2] = smartex.acc.y[maxIndex];
                smartex.storage[3] = smartex.acc.z[maxIndex];
            }
            else if (message.Contains("piezo"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length - 1); //removes last }
                message = message.Remove(0, 13); //removes {"piezo": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.piezo);

                maxValue = smartex.piezo.time.Max();
                maxIndex = smartex.piezo.time.ToList().IndexOf(maxValue);
                smartex.storage[4] = smartex.piezo.value[maxIndex];
            }
            else if (message.Contains("quality"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length - 1); //removes last }
                message = message.Remove(0, 15); //removes {"quality": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.quality);

                maxValue = smartex.quality.time.Max();
                maxIndex = smartex.quality.time.ToList().IndexOf(maxValue);
                smartex.storage[5] = smartex.quality.hr[maxIndex];
                try
                {
                    smartex.storage[6] = smartex.quality.br[maxIndex];
                }
                catch
                {
                    smartex.storage[6] = 0;
                }
                
            }
            else if (message.Contains("hr"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length - 1); //removes last }
                message = message.Remove(0, 10); //removes {"hr": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.hr);

                maxValue = smartex.hr.time.Max();
                maxIndex = smartex.hr.time.ToList().IndexOf(maxValue);
                smartex.storage[7] = smartex.hr.value[maxIndex];
            }
            else if(message.Contains("br"))
            {
                int length = message.Length; //get string length
                message = message.Remove(length - 1); //removes last }
                message = message.Remove(0, 10); //removes {"br": 
                //Debug.Log(message);
                JsonUtility.FromJsonOverwrite(message, smartex.br);

                maxValue = smartex.br.time.Max();
                maxIndex = smartex.br.time.ToList().IndexOf(maxValue);
                smartex.storage[8] = smartex.br.value[maxIndex];
            }
        }

       

    }
}

//take care of array allocation
#region Sensor Classes Definition

/// <summary>
/// /// Remove [] from the initial string ___
/// power: type: float - range: 0 .. 10000 - units: watt rate: 1/10 Hz only if some power is measured
/// </summary>
[System.Serializable]
public class PowerSensor
{
    public float powerwatt;
    public long time;
}

/// <summary>
/// Remove [] from the initial string ___
/// power: type: float - range: 0 .. 10000 - units: watt rate: 1/10 Hz only if some power is measured
/// </summary>
[System.Serializable]
public class ForceSensor
{
    public float forceg;
    public long time;
}

/// <summary>
/// Remove [] and ° from the initial string ____
/// type: float - range: 0 .. 1000 - units: ml/second ( 1000 = 1 l/s ) - rate: 1 Hz only if water is flowing 
/// </summary>
[System.Serializable]
public class WaterSensor
{
    public float coldWaterml;
    public float hotWaterml;
    public float tempWaterC;
    public long time;
}

/// <summary>
///type: int - range: 0 .. 1024 units: 0 = no pressure - rate:  20Hz only if pressure is applied
/// </summary>
[System.Serializable]
public class PressureTable
{
    public int val0;
    public int val1;
    public int val2;
    public int val3;
    public long time;
}

/// <summary>
///type: int - range: 0 .. 1024 units: 0 = no pressure - rate:  20Hz only if pressure is applied
/// </summary>
[System.Serializable]
public class PressureWC
{
    public int frontLeft;
    public int frontRight;
    public int rearLeft;
    public int rearRight;
    public int supportLeft;
    public int supportRight;
    public long time;
}

/// <summary>
///Type: PNG 16UC1 saved as base64 string - range: 0 .. 65000 units: 0 = no pressure - rate:  20Hz only if pressure is applied
/// </summary>
[System.Serializable]
public class PressureMatrix
{
    public string pngBase64;
    public long time;
}

/// <summary>
///Type: float string - range: see pdf units: see pdf - rate: 100Hz
/// </summary>
[System.Serializable]
public class IMU
{
    public float[] accelerations;
    public float quaternions;
    public long time;
}


/// <summary>
///Takes different json strings into the same empatica class. Time var will be overwritten every time
/// </summary>
[System.Serializable]
public class Empatica
{
    public float[] accelerations;
    public float gsr;
    public float tmp;
    public long time;
}

/// <summary>
///Takes different json strings into the same Smartex class organized in structs
/// </summary>
[System.Serializable]
public class Smartex 
{
    public Ecg ecg;
    public Acc acc;
    public Piezo piezo;
    public Quality quality;
    public Hr hr;
    public Br br;

    //[ecg.value , acc.x, acc.y, acc.z, piezo.value, quality.hr, quality.br, hr.value, br.value]
    public int[] storage = new int[9];

    //constructor
    public Smartex()
    {
        ecg = new Ecg();
        acc = new Acc();
        piezo = new Piezo();
        quality = new Quality();
        hr = new Hr();
        br = new Br();
    }
    [System.Serializable]
    public class Ecg
    {
        public int[] value;
        public float[] time;
    };
    [System.Serializable]
    public class Acc
    {
        public int[] x;
        public int[] y;
        public int[] z;
        public float[] time;
    };
    [System.Serializable]
    public class Piezo
    {
        public int[] value;
        public float[] time;
    };
    [System.Serializable]
    public class Quality
    {
        public int[] hr;
        public int[] br;
        public float[] time;
    };
    [System.Serializable]
    public class Hr
    {
        public int[] value;
        public float[] time;
    };
    [System.Serializable]
    public class Br
    {
        public int[] value;
        public float[] time;
    };

    
}

#endregion




