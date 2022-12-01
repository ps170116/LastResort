using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeveloperConsole;
using Unity.VisualScripting;

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

    public float rotX;
    public float rotY;

    public bool camEnabled = true;

    public static CameraController instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


    }

    // Update is called once per frame
    void Update()
    {
        if(camEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            mouseX = Input.GetAxis("Mouse X") * sensX;
            mouseY = Input.GetAxis("Mouse Y") * sensY;

            rotX -= mouseY;
            rotY += mouseX;

            rotX = Mathf.Clamp(rotX, minX, maxX);

            cam.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
 
    }


    public void EnableCam(bool enabled)
    {
        camEnabled = enabled;
    }

    [ConCommand("set_sensX", "set the X sensitivity of camera")]
    public static void SetSensX(int sens = 5)
    {
        instance.sensX = sens;   
    }

    [ConCommand("set_sensY", "set the Y sensitivity of camera")]
    public static void SetSensY(int sens = 5)
    {
        instance.sensY = sens;
    }
}
