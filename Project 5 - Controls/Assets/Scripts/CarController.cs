using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    public GameController game;
    public GameObject playerCar;
    public GameObject cam;
    public float torque = 1.0f;
    public bool clutchDown = false;
    public bool clutchEngaged = false;
    public bool gasPedal = false;
    public int gearEnabled = 0;
    public float rpm = 1000.0f;

    float speed = 0.0f;
    bool slipping = false;
    float slippage = 0.0f;
    float slipRpm = 1000.0f;
    float clutchPenalty = 0.0f;

    int lastGear = 0;

    bool playing = false;
    float[] breaker = new float[1];

    public GameObject spedometer;
    public GameObject tachometer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clutchDown)
        {
            if (gasPedal)
            {
                UpdateRpm(1);
                
            }
            else
            {
                UpdateRpm(-1);
            }
        }
        else if (!clutchEngaged && playing == true)
        {
            if (slipping == false)
            {
                EngageClutch();
            }
            
            if (slipping)
            {
                if (slippage > 0.0f)
                {
                    slippage -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("EndSlip");
                    slipping = false;
                    clutchEngaged = true;
                    rpm = slipRpm;
                }
            }

        }
        
        if (clutchEngaged && playing)
        {
            if (gasPedal)
            {
                UpdateRpm(1);
                GetVelocity();
            }
            else
            {
                UpdateRpm(-1);
            }

        }

        speed *= Mathf.Pow(0.95f, Time.deltaTime);
        playerCar.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
        spedometer.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)playerCar.GetComponent<Rigidbody>().velocity.z).ToString();
        cam.gameObject.transform.position = new Vector3(5, 5, playerCar.transform.position.z - 7.0f);

        if (playerCar.GetComponent<Rigidbody>().transform.position.z > 3000.0f)
        {
            Debug.Log("EnemyWins");
            game.EndRace(0);
        }

        if (playing == false)
        {
            playerCar.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    public void StartRace()
    {
        playing = true;
    }

    public void PressBreak()
    {
        speed *= Mathf.Pow(0.7f, Time.deltaTime);
        playerCar.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);
    }

    void UpdateRpm(int direction)
    {
        float rpmAdj = 0.0f;
        /*
         switch (gearEnabled)
        {
            case -1: // reverse
                torqueAdj = 0.0f;
                break;
            case 0: // neutral
                torqueAdj = 2000.0f;
                break;
            case 1:
                torqueAdj = (2.3f * 60.0f) / 0.15f;
                break;
            case 2:
                torqueAdj = (1.6f * 60.0f) / 0.15f;
                break;
            case 3:
                torqueAdj = (1.2f * 60.0f) / 0.15f;
                break;
            case 4:
                torqueAdj = (1.0f * 60.0f) / 0.15f;
                break;
            case 5:
                torqueAdj = (0.8f * 60.0f) / 0.15f;
                break;
            case 6:
                torqueAdj = (0.6f * 60.0f) / 0.15f;
                break;
        }
        //((1.0f + speed) *
        if (direction == 1)
        {
            float newRpm = rpm + (1.0f * torque * torqueAdj * Time.deltaTime);
            if (newRpm > 8000)
            {
                newRpm = 8000.0f;
            }
            rpm = newRpm;
        }
        else if (direction == -1)
        {
            float newRpm = rpm - (50.0f * torque * torqueAdj * Time.deltaTime);
            if (newRpm < 1000.0f)
            {
                newRpm = 1000.0f;

            }
            rpm = newRpm;
        }
         */
        switch (gearEnabled)
        {
            case -1: // reverse
                // you totaled the car
                break;
            case 0: // neutral
                rpmAdj = 8.0f * torque * Time.deltaTime;
                break;
            case 1:
                rpmAdj = 2.3f * 2.3f * torque * Time.deltaTime;
                break;
            case 2:
                rpmAdj = 1.6f * 1.6f * torque * Time.deltaTime;
                break;
            case 3:
                rpmAdj = 1.2f * 1.2f * torque * Time.deltaTime;
                break;
            case 4:
                rpmAdj = 1.0f * torque * Time.deltaTime;
                break;
            case 5:
                rpmAdj = 0.8f * 0.8f * torque * Time.deltaTime;
                break;
            case 6:
                rpmAdj = 0.6f * 0.6f * torque * Time.deltaTime ;
                break;
        }
        //* (1000.0f / rpm)
        //((1.0f + speed) *
        if (clutchDown || gearEnabled == 0)
        {
            rpmAdj = 10.0f * torque * Time.deltaTime;
            if (direction == 1)
            {
                float newRpm = rpm + rpmAdj;
                if (newRpm > 8000)
                {
                    newRpm = 8000.0f;
                }
                rpm = newRpm;
            }
            else if (direction == -1)
            {
                float newRpm = rpm - (rpmAdj / 8.0f);
                if (newRpm < 1000.0f)
                {
                    newRpm = 1000.0f;

                }
                rpm = newRpm;
            }
        }
        else
        {
            if (direction == 1)
            {
                float newRpm = rpm + rpmAdj;
                if (newRpm > 8000)
                {
                    newRpm = 8000.0f;
                }
                rpm = newRpm;
            }
            else if (direction == -1)
            {
                float newRpm = rpm - (rpmAdj / 2.0f);
                if (newRpm < 1000.0f)
                {
                    newRpm = 1000.0f;

                }
                rpm = newRpm;
            }
        }
        
        


        tachometer.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)(rpm)).ToString();
    }

    public void changeGear(int gear)
    {
        /*
        if (gearEnabled == 0)
        {
            gearEnabled = gear;
            EngageClutch();
        }
        */
        gearEnabled = gear;
        if (gear != 0 && !clutchDown)
        {
            EngageClutch();
        }
    }

    void EngageClutch()
    {
        float newRpm = 0.0f;
        /*
         switch (gearEnabled)
        {
            case -1: // reverse
                newRpm = 0.0f;
                break;
            case 0: // neutral
                newRpm = rpm;
                break;
            case 1:
                newRpm = 5.0f - Mathf.Log((120.0f/(speed*2.3f)) - 1.0f, 2.718282f);
                break;
            case 2:
                newRpm = 100.0f + (speed * (1.6f * 60.0f)) / 0.15f;
                break;
            case 3:
                newRpm = 100.0f + (speed * (1.2f * 60.0f)) / 0.15f;
                break;
            case 4:
                newRpm = 100.0f + (speed * (1.0f * 60.0f)) / 0.15f;
                break;
            case 5:
                newRpm = 100.0f + (speed * (0.8f * 60.0f)) / 0.15f;
                break;
            case 6:
                newRpm = 100.0f + (speed * (0.6f * 60.0f)) / 0.15f;
                break;
        }
         */
        if (speed < 1.0f && lastGear == 0 && gearEnabled > 0)
        {
            speed = 1.0f;
        }

        Debug.Log("speed before clutch engage = " + speed);
        switch (gearEnabled)
        {
            case -1: // reverse
                // engine break
                break;
            case 0: // neutral
                newRpm = rpm - 1000.0f;
                break;
            case 1:
                if (speed > 52.1f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 2.3f)) - 1.0f, 2.718282f));
                break;
            case 2:
                if (speed > 74.9f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 1.6f)) - 1.0f, 2.718282f));
                break;
            case 3:
                if (speed > 99.9f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 1.2f)) - 1.0f, 2.718282f));
                break;
            case 4:
                if (speed > 119.9f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 1.0f)) - 1.0f, 2.718282f));
                break;
            case 5:
                if (speed > 149.9f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 0.8f)) - 1.0f, 2.718282f));
                break;
            case 6:
                if (speed > 199.9f)
                {
                    // break engine
                    Debug.Log("Broke Engine");
                }
                newRpm = 700.0f * (5.0f - Mathf.Log((12.0f / ((speed / 10.0f) * 0.6f)) - 1.0f, 2.718282f));
                break;
        }
        newRpm += 1000.0f;
        Debug.Log("new rpm: " + newRpm);
        if (newRpm < 1000.0f)
        {
            rpm = 1000;
            clutchEngaged = true;
            Debug.Log("new rpm: " + newRpm);
        }
        else if (newRpm > 8000.0f)
        {
            rpm = 8000;
            clutchEngaged = true;
            Debug.Log("new rpm: " + newRpm);
        }
        
        else if (newRpm <= rpm)
        {
            // slippage
            slippage = Mathf.Pow((rpm/newRpm), 2) * 0.2f * clutchPenalty;
            slipRpm = newRpm * (newRpm / rpm);
            rpm = newRpm + (0.5f * (rpm - newRpm));
            clutchEngaged = false;
            slipping = true;
            Debug.Log("slip rpm = " + slipRpm);
        }
        else
        {
            // slippage
            slippage = Mathf.Pow((newRpm/rpm), 2) * 0.1f * clutchPenalty;
            slipRpm = newRpm;
            rpm = newRpm - (0.5f * (newRpm - rpm));
            clutchEngaged = false;
            slipping = true;
            Debug.Log("slip rpm = " + slipRpm);
        }
        
    }
    
    public void PressClutch(bool state)
    {
        clutchDown = state;
        if (!clutchDown)
        {
            EngageClutch();
            clutchPenalty = 1.0f;
        }
        else if (clutchDown)
        {
            clutchEngaged = false;
            clutchPenalty = 0.25f;
        }
    }
    public void PressGas(bool state)
    {
        gasPedal = state;
    }

    void GetVelocity()
    {
        float vel = 0.0f; // meters per second
        /*switch (gearEnabled)
        {
            case -1: // reverse
                break;
            case 0: // neutral
                vel = speed;
                break;
            case 1:
                vel = (0.15f * rpm) / (2.3f * 60.0f);
                break;
            case 2:
                vel = (0.15f * rpm) / (1.6f * 60.0f);
                break;
            case 3:
                vel = (0.15f * rpm) / (1.2f * 60.0f);
                break;
            case 4:
                vel = (0.15f * rpm) / (1.0f * 60.0f);
                break;
            case 5:
                vel = (0.15f * rpm) / (0.8f * 60.0f);
                break;
            case 6:
                vel = (0.15f * rpm) / (0.6f * 60.0f);
                break;
        }
        speed = vel;*/
        switch (gearEnabled)
        {
            case -1: // reverse
                break;
            case 0: // neutral
                vel = speed;
                break;
            case 1:
                vel = 120.0f / (2.3f*(1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f/7000.0f)) )));
                break;
            case 2:
                vel = 120.0f / (1.6f * (1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f / 7000.0f)))));
                break;
            case 3:
                vel = 120.0f / (1.2f * (1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f / 7000.0f)))));
                break;
            case 4:
                vel = 120.0f / (1.0f * (1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f / 7000.0f)))));
                break;
            case 5:
                vel = 120.0f / (0.8f * (1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f / 7000.0f)))));
                break;
            case 6:
                vel = 120.0f / (0.6f * (1 + Mathf.Pow(2.718282f, 5.0f - ((rpm - 1000.0f) * (10.0f / 7000.0f)))));
                break;
        }
        speed = vel;
        playerCar.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, vel);
    }


    // info
    // wheel (.15 meters per rotation)
    // ratio (engine rot per wheel rot)
    // 1st (2.3)
    // 2st (1.6)
    // 3st (1.2)
    // 4st (1)
    // 5st (0.8)
    // 6st (0.6)

    // higher gears have more torque
    // higher rpm has more torque
    // more torque means more acceleration
    // use _/- function


    public void ResetRace()
    {
        playing = false;
        speed = 0.0f;
        playerCar.gameObject.transform.position = new Vector3(4, 0, 10);
        clutchDown = false;
        clutchEngaged = false;
        gasPedal = false;
        gearEnabled = 0;
        rpm = 1000.0f;

        slipping = false;
        slippage = 0.0f;
        slipRpm = 1000.0f;
        clutchPenalty = 0.0f;

         lastGear = 0;


    }

}
