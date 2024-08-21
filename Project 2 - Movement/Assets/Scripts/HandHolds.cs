using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHolds : MonoBehaviour
{
    //public PlayerController player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("LeftHand"))
        {
            //player.HandCanGrab(0, true);
            Debug.Log("Hand can grab");
        }
        if (collision.collider.gameObject.CompareTag("RightHand"))
        {
            //player.HandCanGrab(1, true);
            Debug.Log("Hand can grab");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("LeftHand"))
        {
            //player.HandCanGrab(0, false);
            Debug.Log("no grab");
        }
        if (collision.collider.gameObject.CompareTag("RightHand"))
        {
            //player.HandCanGrab(1, false);
            Debug.Log("no grab");
        }
    }
}
