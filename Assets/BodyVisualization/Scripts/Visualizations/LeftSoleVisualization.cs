using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftSoleVisualization : MonoBehaviour, IHideable
{
    private SoleDataManager m_soleDataManager;

    public GameObject[] LeftSoleParts = new GameObject[16];

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
        m_soleDataManager = FindObjectOfType<SoleDataManager>();
    }

    private void Update()
    {
        for (int i = 0; i < 16; i++)
        {
            float value = (float)m_soleDataManager.LeftSole[i] / 255;

            if (value <= 0.5) LeftSoleParts[i].GetComponent<SpriteRenderer>().color = new Color(value * 2, 1, 0);
            else
            {
                value -= 0.5f;
                LeftSoleParts[i].GetComponent<SpriteRenderer>().color = new Color(1, 1 - (value * 2), 0);
            }

        }
    }
}

