using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Transmission transmission;
    public CarController car;
    float xVal;
    float yVal;

    float[] mousePos;

    float screenRatio = 1.0f;

    bool shifting = false;
    // Start is called before the first frame update
    void Start()
    {
        mousePos= new float[2];
        screenRatio = (float)Screen.width / 1920.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shifting)
        {
            UpdateMousePos();
            transmission.Shift(mousePos);
        }
    }

    public float[] GetMouseData()
    {
        
        return mousePos;
    }

    public void StartShifting()
    {
        shifting = true;
    }

    public void EndShifting()
    {
        if (shifting)
        {
            shifting = false;
        }
    }

    void UpdateMousePos()
    {
        // 510 + 960, 200 + 540 === 1470, 
        mousePos[0] = (Input.mousePosition.x/screenRatio) - 1470;
        mousePos[1] = (Input.mousePosition.y/screenRatio) - 740;
        // if x < -250, x = 250
        // if -250 < x < -230, y can be greater than 0
        // if -90 < x < -70, y can be greater than or less than 0
        // if 70 < x < 90, y can be greater than or less than 0
        // if 230 < x < 250, y can be greater than or less than 0
        /*
        if (mousePos[0] < -230)
        {
            // shiftnob.x = -240
            // can have positive y
        }
        else if (-90 <= mousePos[0] && mousePos[0] <= -70)
        {

        }
        else if (70 <= mousePos[0] && mousePos[0] <= 90)
        {

        }
        else if (230 <= mousePos[0])
        {

        }
        */
        CheckGear();
    }

    void CheckGear()
    {
        // if knob is close enough, tell car that gear is engaged
    }

    public void ResetRace()
    {
        shifting= false;
    }
}
