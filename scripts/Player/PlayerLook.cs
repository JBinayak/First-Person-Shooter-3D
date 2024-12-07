using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensativity = 30f;
    public float ySensativity = 30f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void processLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        // calc camera roation for up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensativity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // apply to camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // look for left and right
        transform.Rotate(Vector3.up * (Time.deltaTime * mouseX) * xSensativity);
    }
}
