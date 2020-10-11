using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float rotationSpeed = 10;

    float xRotation = 0f;

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        xRotation -= vertical;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        gameObject.transform.Rotate(Vector3.up * horizontal);
        transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }

}
