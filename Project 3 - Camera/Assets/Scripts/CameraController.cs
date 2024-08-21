using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject mainCam;
    Vector3 camera1;
    Vector3 camera2;
    Vector3 camera3;
    Vector3 camera4;
    Vector3 camera5;
    Vector3 camera6;

    public GameObject camlight1;
    public GameObject camlight2;
    public GameObject camlight3;
    public GameObject camlight4;
    public GameObject camlight5;
    public GameObject camlight6;

    Color darkRed = new Color(160, 0, 0);
    Color lightGreen = new Color(0, 255, 0);

    float swapTimer = 0.0f;

    int currentCam = 1;
    // Start is called before the first frame update
    void Start()
    {
        camera1 = new Vector3(2.0f, 10.0f, 5.7f);
        camera2 = new Vector3(11.6f, 10.0f, 5.7f);
        camera3 = new Vector3(2.0f, 10.0f, 15.0f);
        camera4 = new Vector3(11.6f, 10.0f, 15.0f);
        camera5 = new Vector3(2.0f, 10.0f, 24.3f);
        camera6 = new Vector3(11.6f, 10.0f, 24.3f);
        SwapCamera(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (swapTimer > 0.0f)
        {
            swapTimer -= Time.deltaTime;
        }
        else
        {
            if      ((Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)) && currentCam != 1)
            {
                SwapCamera(1);
                swapTimer = 0.5f;
            }
            else if ((Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) && currentCam != 2)
            {
                SwapCamera(2);
                swapTimer = 0.5f;
            }
            else if ((Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)) && currentCam != 3)
            {
                SwapCamera(3);
                swapTimer = 0.5f;
            }
            else if ((Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)) && currentCam != 4)
            {
                SwapCamera(4);
                swapTimer = 0.5f;
            }
            else if ((Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)) && currentCam != 5)
            {
                SwapCamera(5);
                swapTimer = 0.5f;
            }
            else if ((Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)) && currentCam != 6)
            {
                SwapCamera(6);
                swapTimer = 0.5f;
            }
        }
        
    }

    public void SwapCamera(int cam)
    {
        camlight1.GetComponent<Image>().color = darkRed;
        camlight2.GetComponent<Image>().color = darkRed;
        camlight3.GetComponent<Image>().color = darkRed;
        camlight4.GetComponent<Image>().color = darkRed;
        camlight5.GetComponent<Image>().color = darkRed;
        camlight6.GetComponent<Image>().color = darkRed;
        switch (cam)
        {
            case 1:
                mainCam.transform.position = camera1;
                camlight1.GetComponent<Image>().color = lightGreen;
                break; 
            case 2:
                mainCam.transform.position = camera2;
                camlight2.GetComponent<Image>().color = lightGreen;
                break; 
            case 3:
                mainCam.transform.position = camera3;
                camlight3.GetComponent<Image>().color = lightGreen;
                break; 
            case 4: 
                mainCam.transform.position = camera4;
                camlight4.GetComponent<Image>().color = lightGreen;
                break; 
            case 5:
                mainCam.transform.position = camera5;
                camlight5.GetComponent<Image>().color = lightGreen;
                break;
            case 6:
                mainCam.transform.position = camera6;
                camlight6.GetComponent<Image>().color = lightGreen;
                break;
        }
        currentCam = cam;
    }
}
