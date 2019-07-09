using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataVisualizationManager
{
    public class DataVisualizationInfo
    {
        public string name;
        public Vector3 position;
        public AbstractVisualization visualization;
    }

    private Dictionary<string, DataVisualizationInfo> m_dataVisualizationInfo;

    public DataVisualizationManager()
    {
        m_dataVisualizationInfo = new Dictionary<string, DataVisualizationInfo>();
    }

    public void OnUpdate()
    {
        foreach (string dataName in m_dataVisualizationInfo.Keys)
        {
            AbstractVisualization visualization = m_dataVisualizationInfo[dataName].visualization;
            if (visualization != null)
            {
                object data = DataStore.Instance.GetData(dataName);
                if (data != null)
                {
                    m_dataVisualizationInfo[dataName].visualization.UpdateProperty("value", data);
                }
            }
        }
    }

    public void RegisterDataName(string dataName)
    {
        if (m_dataVisualizationInfo.ContainsKey(dataName))
        {
            return;
        }

        DataVisualizationInfo info = new DataVisualizationInfo
        {
            name = dataName,
            position = Vector3.zero,
            visualization = null
        };

        m_dataVisualizationInfo.Add(dataName, info);
    }

    public void UnregisterDataName(string dataName, bool deleteVisualization = true)
    {
        if (m_dataVisualizationInfo.ContainsKey(dataName))
        {
            if (deleteVisualization)
            {
                GameObject.Destroy(m_dataVisualizationInfo[dataName].visualization.gameObject);
            }

            m_dataVisualizationInfo[dataName].visualization = null;
            m_dataVisualizationInfo.Remove(dataName);
        }
    }

    public bool IsDataNameRegistered(string dataName)
    {
        return m_dataVisualizationInfo.ContainsKey(dataName);
    }

    public bool GetVisualizationPosition(string dataName, out Vector3 position)
    {
        if (m_dataVisualizationInfo.ContainsKey(dataName))
        {
            position = m_dataVisualizationInfo[dataName].position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    public void SetVisualizationPosition(string dataName, Vector3 position)
    {
        if (!m_dataVisualizationInfo.ContainsKey(dataName))
        {
            RegisterDataName(dataName);
        }

        m_dataVisualizationInfo[dataName].position = position;
    }

    public void SetVisualization(string dataName, AbstractVisualization visualization)
    {
        if (!m_dataVisualizationInfo.ContainsKey(dataName))
        {
            RegisterDataName(dataName);
        }

        m_dataVisualizationInfo[dataName].visualization = visualization;
    }

    public AbstractVisualization GetDataVisualization(string dataName)
    {
        if(m_dataVisualizationInfo.ContainsKey(dataName))
        {
            return m_dataVisualizationInfo[dataName].visualization;
        }

        return null;
    }
}
