using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    
    public Rigidbody rb;
    public bool hasFired = false;
    public PlayerController player;
    bool hit = false;
    bool freeze = false;

    // Start
    void Start()
    {
        
    }

    // Updates the movement of the arrow.
    void Update()
    {
        // Adjusts the angle of the arrow in flight to ensure a propper arc.
        if (hasFired)
        {
            float rot = Mathf.Atan2(rb.velocity.y , rb.velocity.x) * 57.29578f;
            rb.transform.rotation = Quaternion.Euler(0,0,rot);
            Debug.Log("Velocity: ( " + rb.velocity.x + ", " + rb.velocity.y + " ), Angle: " + rot);
        }
        // Double check that the arrow is not moving after initial hit
        if (hit)
        {
            hit = false;
            hasFired = false;
            freeze = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Checks what the arrow hits
    private void OnCollisionEnter(Collision collision)
    {
        // if the arrow is moving and has not collided, checks what it hit.
        if (freeze == false && hit == false)
        {
            // Tells the player controller what object was hit.
            if (collision.collider.CompareTag("John"))
            {
                player.ArrowHit(collision.gameObject.tag);
            }
            else
            {
                player.ArrowHit("none");
            }

            // Freezes the arrow in place, and ends collision detection.
            hit = true;
            hasFired = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            
        }
        
    }

    // Lets the player controller know if the arrow hits the rope.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Rope"))
        {
            player.ArrowHit(collision.gameObject.tag);
        }
    }
}
