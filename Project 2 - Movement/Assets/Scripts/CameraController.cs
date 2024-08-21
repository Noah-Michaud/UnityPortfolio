using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public PlayerController player;

    public float cameraOffset = 1.0f;

    float lastPos;
    float camPos;
    bool updating = false;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = mainCamera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (updating)
        {
            if (mainCamera.transform.position.y - camPos < 0.05f && mainCamera.transform.position.y - camPos > -0.05f)
            {
                mainCamera.transform.position = new Vector3(0, camPos, -10);
                updating = false;
                Debug.Log("stopped UpdatingCamera");
            }
            else
            {
                mainCamera.transform.position += new Vector3(0, (camPos - lastPos) * Time.deltaTime, 0);
            }
            
            
        }
    }

    public void UpdateCamera(float left, float right, float lPos, float rPos)
    {
        if (left+right >= 1)
        {
            lastPos = mainCamera.transform.position.y;
            camPos = (((left*lPos) + (right*rPos))/(left+right)) - cameraOffset;
            updating = true;
            Debug.Log("UpdatingCamera from " + lastPos + " to " + camPos);
        }
    }

    public void ResetCamera(float left, float right)
    {
        mainCamera.transform.position = new Vector3(0, ((left + right) / 2.0f) - cameraOffset, -10);
        lastPos = mainCamera.transform.position.y;
        camPos = mainCamera.transform.position.y;
        updating = false;
    }
}
