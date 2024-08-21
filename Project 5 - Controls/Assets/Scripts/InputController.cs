using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameController game;
    bool playing = false;
    public CarController car;
    bool alreadyPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if(Input.GetKey(KeyCode.Q))
            {
                //clutch
                car.PressClutch(true);
                alreadyPressed = false;
            }
            else
            {
                if (!alreadyPressed)
                {
                    car.PressClutch(false);
                    alreadyPressed = true;
                }
                
            }
            if (Input.GetKey(KeyCode.W))
            {
                //break
                car.PressBreak();
            }

            if (Input.GetKey(KeyCode.E))
            {
                //gas
                car.PressGas(true);
            }
            else
            {
                car.PressGas(false);
            }


        }
        else
        {
            if (Input.GetKey(KeyCode.R))
            {
                game.Ready();
                playing = true;
            }
        }
    }

    public void ResetRace()
    {
        playing = false;
    }


}
