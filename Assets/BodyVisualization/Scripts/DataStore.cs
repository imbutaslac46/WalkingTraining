using System.Collections.Generic;
using UnityEngine;

public class DataStore : MonoBehaviour
{
    public PowerSensor powerSensorBed;
    public Empatica empatica;
    public Smartex smartex = new Smartex();

    public List<string> chairSensors;

    private Dictionary<string, object> m_data;

    public static DataStore Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public object GetData( string dataName )
    {
        if (m_data.ContainsKey(dataName))
        {
            return m_data[dataName];
        }

        return null;
    }

    public void SetData( string dataName, object data )
    {
        if (!m_data.ContainsKey(dataName))
        {
            m_data.Add(dataName, data);
        }
        else
        {
            m_data[dataName] = data;
        }
    }

    private void Start()
    {
        m_data = new Dictionary<string, object>();

        chairSensors = new List<string>();
    }
}