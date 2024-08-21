using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public PlayerController player;
    public Camera mainCam;
    float xDist;
    float yDist;

    float[] mousePos = new float[2]{0.0f, 0.0f }; 

    public GameObject leftPtr;
    public GameObject rightPtr;
    public SpringJoint leftPtrSp;
    public SpringJoint rightPtrSp;

    public float upReachDist = 5.0f;
    public float sideReachDist = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        xDist = 17.8f * (mainCam.orthographicSize / 5.0f);
        yDist = 10.0f * (mainCam.orthographicSize / 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // -8.9 n 8.9 , -5 n 5, screen (0,0), size = 5
        mousePos[0] = xDist*(Input.mousePosition.x / Screen.currentResolution.width) - (xDist / 2.0f);
        mousePos[1] = yDist*(Input.mousePosition.y / Screen.currentResolution.height) - (yDist / 2.0f) + mainCam.transform.position.y;

        float lPtrX = mousePos[0];
        float lPtrY = mousePos[1];
        float rPtrX = mousePos[0];
        float rPtrY = mousePos[1];

        if (player.rightHand.transform.position.y + upReachDist < mousePos[1])
        {
            lPtrY = player.rightHand.transform.position.y + upReachDist;
        }
        if (player.rightHand.transform.position.x + sideReachDist < mousePos[0])
        {
            lPtrX = player.rightHand.transform.position.x + sideReachDist;
        }
        else if (player.rightHand.transform.position.x - sideReachDist > mousePos[0])
        {
            lPtrX = player.rightHand.transform.position.x - sideReachDist;
        }

        if (player.leftHand.transform.position.y + upReachDist < mousePos[1])
        {
            rPtrY = player.leftHand.transform.position.y + upReachDist;
        }
        if (player.leftHand.transform.position.x + sideReachDist < mousePos[0])
        {
            rPtrX = player.leftHand.transform.position.x + sideReachDist;
        }
        else if (player.leftHand.transform.position.x - sideReachDist > mousePos[0])
        {
            rPtrX = player.leftHand.transform.position.x - sideReachDist;
        }


        


        leftPtr.transform.position = new Vector3(lPtrX, lPtrY, -3.0f);
        rightPtr.transform.position = new Vector3(rPtrX, rPtrY, -3.0f);

    }

    public float[] GetMouseData()
    {
        float[] data = new float[4] { 0.0f, 0.0f, -1.0f, -1.0f };
        data[0] = mousePos[0];
        data[1] = mousePos[1];
        if(Input.GetMouseButton(0))
        {
            data[2] = 1.0f;
        }
        if (Input.GetMouseButton(1))
        {
            data[3] = 1.0f;
        }
        return data;

    }

    public void EnableSprings(int spring, int turnedOn)
    {
        if (spring == 0)
        {
            leftPtrSp.spring = 150.0f * turnedOn;
        }
        if (spring == 1)
        {
            rightPtrSp.spring = 150.0f * turnedOn;
        }
    }

    
}
