using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFrontOnEnable : MonoBehaviour
{
    public float distance = 1.0f;

    private void OnEnable()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        transform.position = Camera.main.transform.position + forward * distance;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}
