using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSoleTransform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPosition(Vector3 position)
    {
        this.transform.localPosition = position;
    }
    public void SetRotation(Vector3 rotation)
    {
        this.transform.localEulerAngles = rotation;
    }
    public void SetScale(Vector3 scale)
    {
        this.transform.localScale = scale;
    }
}
