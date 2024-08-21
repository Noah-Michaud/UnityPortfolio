using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public HandHoldSpwner holdSpawner;
    public CameraController mainCamera;

    public GameObject pauseMenu;
    public GameObject mainMenu;

    public GameObject maxHeightText;
    public GameObject currentHeightText;

    float maxHeight = 0.0f;
    float currentHeight = 0.0f;

    bool paused = true;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        UpdateScores();
        
    }


    public void StartGame()
    {
        currentHeight = 0.0f;
        UpdateScores();
        player.ResetPlayer();
        holdSpawner.ResetHolds();

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        paused = false;
    }

    public void EndGame()
    {
        player.PausePlayer(true);
        mainMenu.SetActive(true);
        paused = true;
    }

    public void PauseGame()
    {
        player.PausePlayer(true);
        pauseMenu.SetActive(true);
        paused = true;
    }

    public void ResumeGame()
    {
        //player.PausePlayer(false);
        pauseMenu.SetActive(false);
        paused = false;
    }

    public void ResetGame()
    {
        currentHeight = 0.0f;
        UpdateScores();
        player.ResetPlayer();
        holdSpawner.ResetHolds();

        pauseMenu.SetActive(false);
        paused = false;
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void UpdateHeight(float left, float right)
    {
        if (left >= right)
        {
            currentHeight = left - 2.4f;
        }
        else
        {
            currentHeight = right - 2.4f;
        }

        if (currentHeight > maxHeight)
        {
            maxHeight = currentHeight;
        }

        UpdateScores();
    }

    void UpdateScores()
    {
        maxHeightText.GetComponent<TMPro.TextMeshProUGUI>().text = getRoundedDecimal(maxHeight);
        currentHeightText.GetComponent<TMPro.TextMeshProUGUI>().text = getRoundedDecimal(currentHeight);
    }

    string getRoundedDecimal(float num)
    {
        string text;
        int whole = (int)num;
        int dec = (int)((num - whole) * 10.0f);
        if (dec < 10)
        {
            text = whole.ToString() + ".0" + dec.ToString();
        }
        else
        {
            text = whole.ToString() + "." + dec.ToString();
        }
        return text;
    }

    public void TryPause()
    {
        if (paused == false)
        {
            PauseGame();
        }
    }
}
