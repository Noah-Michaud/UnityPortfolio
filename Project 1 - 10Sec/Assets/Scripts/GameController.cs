using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public MouseController mouse;

    bool playing = false;
    float timer = 10.0f;
    bool outOfTime = false;

    bool johnEscape = false;

    public Sprite johnSad;
    public Sprite johnHappy;
    public Sprite johnDead;

    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject gameOverText;

    public GameObject timerTextObject;

    public GameObject littleJohn;
    public GameObject rope;
    public GameObject gear;
    public GameObject platform;

    // Start 
    void Start()
    {
        
    }

    // Checks current game state and takes input.
    void Update()
    {
        // If the game is running, timer counts down.
        if (playing == true)
        {
            timer -= Time.deltaTime;
            UpdateTimer();
            if (timer <= 1.0f)
            {
                gear.gameObject.transform.rotation = Quaternion.Euler(0, 0, -45.0f + (timer * 90.0f));
            }
            if (timer <= 0.0f)
            {
                EndGame(2);

            }
        }
        if (outOfTime)
        {
            timer -= Time.deltaTime;
            if (timer > 0.0f)
            {
                littleJohn.transform.position = new Vector3(7.5f, 1.9f + (timer * 4.0f), 0);
                rope.transform.position = new Vector3(7.5f, 3.0f + (timer * 4.0f), 0.1f);
            }
            else if (timer <= 0.0f)
            {
                outOfTime= false;
            }


        }
        if (johnEscape)
        {
            if (timer < 0.5f)
            {
                littleJohn.transform.position = new Vector3(7.5f, 2.7f - (timer * 3.2f), 0);
                timer += Time.deltaTime;
            }
            else if (timer < 7.0f)
            {
                littleJohn.transform.position = new Vector3(7.5f - ((timer - 0.5f) * 5.0f), 1.1f, 0);
                timer += Time.deltaTime;
            }
            else if (timer > 7.0f)
            {
                johnEscape = false;
            }
        }
    }

    public void StartGame()
    {
        mainMenu.GetComponent<Canvas>().enabled = false;
        gameOverMenu.GetComponent<Canvas>().enabled = false;
        gear.gameObject.transform.rotation = Quaternion.Euler(0, 0, 45);
        player.playing = true;
        mouse.playing = true;
        playing = true;
        timer = 10.0f;
        UpdateTimer();
        player.DestroyArrows();
        littleJohn.transform.position = new Vector3(7.5f, 2.7f, 0);
        rope.transform.position = new Vector3(7.5f, 3.8f, 0.1f);
        platform.SetActive(true);
        rope.SetActive(true);
        littleJohn.GetComponent<SpriteRenderer>().sprite = johnSad;
        johnEscape = false;
    }

    public void ResetGame()
    {
        StartGame();
    }

    public void HitObject(string hit)
    {
        switch (hit)
        {
            case "Rope":
                EndGame(0);
                break;
            case "John":
                EndGame(1);
                break;
            default:
                break;

        }

    }

    public void EndGame(int type)
    {
        playing = false;
        player.playing = false;
        mouse.playing = false;

        gameOverMenu.GetComponent<Canvas>().enabled = true;
        switch (type)
        {
            case 0:
                gameOverText.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You saved Little John!";
                littleJohn.GetComponent<SpriteRenderer>().sprite = johnHappy;
                johnEscape = true;
                timer = 0.0f;
                rope.SetActive(false);
                break;
            case 1:
                gameOverText.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You killed Little John!";
                littleJohn.GetComponent<SpriteRenderer>().sprite = johnDead;
                break;
            case 2:
                gameOverText.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Little John has been hanged!";
                littleJohn.GetComponent<SpriteRenderer>().sprite = johnDead;
                timer = 0.2f;
                outOfTime = true;
                platform.SetActive(false);
                break;
        }
    }

    void UpdateTimer()
    {
        timerTextObject.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = TimerText();
    }

    string TimerText()
    {
        int whole = (int)(timer);
        int dec = (int)((timer - whole) * 100);
        string text = whole.ToString() + "." + dec.ToString();    

        return text;
    }

}
