using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFromAnchorTowardsCamera : MonoBehaviour
{
    public float offset = 0.1f;

    public Transform anchor;
    
    private void Update()
    {
        Vector3 anchorPosition = Vector3.zero;
        if (anchor != null)
            anchorPosition = anchor.position;
        else if (transform.parent != null)
            anchorPosition = transform.parent.position;

        transform.position = anchorPosition + (Camera.main.transform.position - anchorPosition).normalized * offset;
    }
}
