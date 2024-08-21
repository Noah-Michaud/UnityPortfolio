using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearZone : MonoBehaviour
{
    public int gearNumber = 0;
    public CarController car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ShiftLever")
        {
            car.changeGear(gearNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ShiftLever")
        {
            car.changeGear(0);
        }
    }
}
