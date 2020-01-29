using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpriteToggle : MonoBehaviour {

    private SkeletonManager m_skeletonManager;
    private DrawLineHead m_drawLineHead;
    private GameObject RefHeightCube;

    private void Awake()
    {
        m_skeletonManager = GameObject.FindObjectOfType<SkeletonManager>();
        m_drawLineHead = GameObject.FindObjectOfType<DrawLineHead>();
    }

    public void Start ()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().material.color = new Color (1f, 1f, 1f, 0.5f);

        RefHeightCube = GameObject.Find("RefHeight");
        RefHeightCube.GetComponent<MeshRenderer>().enabled = false;
	}
	
	public void Update ()
    {
        GetComponent<SpriteRenderer>().enabled = m_skeletonManager.RoadFlag;
        RefHeightCube.GetComponent<MeshRenderer>().enabled = m_skeletonManager.HeightFlag;
    }

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
    public void SetRotation(Vector3 rotation)
    {
        transform.localEulerAngles = rotation;
    }
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    public void SetHeight(Vector3 position)
    {
        RefHeightCube.transform.localPosition = position;
    }
    public void SetAlpha(float alpha)
    {
        GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, alpha);
    }
    public void SetHeadOffset(Vector3 position)
    {
        m_drawLineHead.headOffset = position;
    }

}
