using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.Antlr3.Runtime;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController game;
    public GameObject player;
    Rigidbody rb;
    public float speed;

    public CameraController cam;



    int vert = 0;
    int horiz = 0;

    bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                game.TryPause();
            }
            vert = 0;
            horiz = 0;

            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                vert = 1;
            }
            else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) || !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                vert = -1;
            }

            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                horiz = -1;
            }
            else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) || !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                horiz = 1;
            }

            if (vert != 0 || horiz != 0)
            {
                //Debug.Log("Trying to move ( " + horiz + ", " + vert + " )");
                Movement(vert, horiz);
            }
            else
            {
                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
        
    }

    void Movement(int vert, int horiz)
    {
        float norm = 1.0f;
        if ((vert + horiz == -2) || (vert + horiz == 0) || (vert + horiz == 2))
        {
            norm = 0.7071f;
        }
        rb.velocity = new Vector3(horiz * speed * norm, 0.0f, vert * speed * norm);
        //Debug.Log("Moving ( " + horiz * speed * norm + ", " + vert * speed * norm + " )");
    }

    public void ResetPosition()
    {
        gameObject.transform.position = new Vector3(1.5f, 0, 1.5f);
    }

    public void PausePlayer(bool gameState)
    {
        if (gameState)
        {
            playing = false;
            rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
        else
        {
            playing = true;
        }
    }

        
}
