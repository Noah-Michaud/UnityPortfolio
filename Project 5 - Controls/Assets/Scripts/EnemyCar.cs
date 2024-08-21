using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public GameController game;
    public GameObject car;
    public bool playing = false;
    public float acceleration = 1.0f;
    Rigidbody rb;
    float vel = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = car.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (vel < 180.0f)
            {
                vel += acceleration * Time.deltaTime;
            }
            else
            {
                vel = 180.0f;
            }
            rb.velocity = new Vector3(0,0,vel);
            Debug.Log("enemy car speed " + vel);
            if (rb.transform.position.z > 3000.0f)
            {
                Debug.Log("EnemyWins");
                game.EndRace(1);
            }
        }
    }

    public void StartRace()
    {
        playing = true;
    }

    public void ResetRace()
    {
        playing = false;
        vel = 0.0f;
        rb.velocity = new Vector3(0, 0, 0);
        car.gameObject.transform.position = new Vector3(0,0,10);
    }
}
