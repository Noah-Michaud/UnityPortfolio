using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject countdownText;
    public CarController playerCar;
    public EnemyCar enemyCar;
    public InputController inputController;
    public GameObject MainMenu;
    public GameObject InfoMenu;
    public GameObject EndScreen;
    public GameObject EndMessage;

    float countdown = 4.0f;
    bool counting = false;
    bool grabbedshift = false;
    bool ready = false;

    
    // Start is called before the first frame update
    void Start()
    {
        countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
        OpenMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedshift && ready)
        {
            countdown = 4.0f;
            counting = true;
            ready = false;
            Debug.Log("start");
        }
        if (counting == true)
        {
            Debug.Log("counting" + countdown);
            countdown -= Time.deltaTime;
            if (countdown >= 3.0f) 
            {
                countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
            }
            else if (countdown >= 2.0f)
            {
                countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
            }
            else if (countdown >= 1.0f)
            {
                countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = "1";
            }
            else if (countdown >= 0.0f) 
            {
                countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = "GO!";
                playerCar.StartRace();
                enemyCar.StartRace();
            }
            else 
            {
                countdownText.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
                counting = false;
            }
        }

        
        
    }


    public void GrabbedShifter()
    {
        grabbedshift = true;
        Debug.Log("grabbed shifter");
    }

    public void Ready()
    {
        ready = true;
        Debug.Log("ready");
    }

    public void OpenMainMenu()
    {
        EndScreen.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
    }

    public void OpenInfoMenu()
    {
        EndScreen.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        EndScreen.gameObject.SetActive(false);
        InfoMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        countdown = 4.0f;
        counting = false;
        grabbedshift = false;
        ready = false;
        playerCar.ResetRace();
        enemyCar.ResetRace();
        inputController.ResetRace();
    }

    public void EndRace(int winner)
    {
        EndScreen.gameObject.SetActive(true);
        InfoMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        if (winner == 0) 
        {
            EndMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "You Won!";
        }
        else if (winner == 1)
        {
            EndMessage.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lost!";
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
