using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camra : MonoBehaviour
{
    public Transform target;
    float smoothSpeed = 0.125f;
    public Vector3 offset;
    private void FixedUpdate()
    {
        Vector3 desierPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desierPos, smoothSpeed);
        transform.position = smoothPos;
        transform.LookAt(target.position);
        
    }
}
