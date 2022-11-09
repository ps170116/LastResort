using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    [SerializeField] float sensX = 5;
    [SerializeField] float sensY = 5;

    [SerializeField] float maxX = 90f;
    [SerializeField] float minX = -90f;



    public float mouseX;
    public float mouseY;

    float rotX;
    float rotY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensX;
        mouseY = Input.GetAxis("Mouse Y") * sensY;

        rotX -= mouseY;
        rotY += mouseX;

        rotX = Mathf.Clamp(rotX, minX, maxX);

        cam.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }
}
