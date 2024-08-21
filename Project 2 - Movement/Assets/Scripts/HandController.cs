using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public PlayerController player;
    public int hand = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HandHold"))
        {
            player.HandCanGrab(hand, true);
            //Debug.Log("Hand can grab");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HandHold"))
        {
            player.HandCanGrab(hand, false);
            //Debug.Log("no grab");
        }
    }

    
}
