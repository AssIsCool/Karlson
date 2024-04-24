using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f; // Mouse sensitivity

    private float rotationX = 0.0f; // Initial rotation around the X-axis

    void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the camera based on the mouse input
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f); // Clamp the rotation to prevent flipping

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // Rotate around the local X-axis
        transform.parent.Rotate(Vector3.up * mouseX); // Rotate the parent (pivot) around the Y-axis
    }
}
