using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    // mouse sensitivity
    public float sensitivityX = 200f;
    public float sensitivityY = 200f;
    public Transform fpsTransform;

    // use variable to clamp rotation
    private float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // lock & remove cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // avoid gimbal lock
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        fpsTransform.rotation *= Quaternion.Euler(Vector3.up * mouseX);
    }
}
