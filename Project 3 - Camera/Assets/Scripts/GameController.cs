using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public EnemyController[] enemies;

    string lastMenu = "";

    public GameObject mainMenu;
    public GameObject gameInfoMenu;
    public GameObject missionDebriefMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public GameObject hudTimer;
    public GameObject hudPrimObj;
    public GameObject hudSecObj;

    public GameObject missionCompleteMenu;
    public GameObject missionTimerVal;
    public GameObject missionPrimObjVal;
    public GameObject missionSecObjVal;
    public GameObject missionTimerScore;
    public GameObject missionPrimObjScore;
    public GameObject missionSecObjScore;
    public GameObject missionScore;
    public GameObject missionGrade;

    int primObjNeed = 1;
    int secObjNeed = 2;
    int primObjCollect = 0;
    int secObjCollect = 0;
    float missionTimer = 0.0f;

    int primObjScore = 0; // 5000 / num of obj
    int secObjScore = 0; // 3000 / num of obj
    int timeScore = 0; // 2300 - 100*(time/30)
    int totalScore = 0;
    string totalGrade = "F"; // 5000 >= d, 6500 >= c, 8000 >= b, 10000 >= a, 10000 < s

    bool paused = true;

    bool playing = false;

    bool playerIsSafe = false;

    bool missionComplete = false;

    float missionCompleteTimer = 0.0f;

    int missionCompleteStep = 0;

    GameObject[] collection;
    private void Update()
    {
        if (!paused)
        {
            missionTimer += Time.deltaTime;
            hudTimer.GetComponent<TMPro.TextMeshProUGUI>().text = GetTimeString(true);
        }
        if (missionComplete)
        {
            missionCompleteTimer += Time.deltaTime;
            if (missionCompleteTimer >= 1.0f)
            {
                if (missionCompleteStep < 8)
                {
                    UpdateMissionEnd(missionCompleteStep);
                    missionCompleteStep++;
                    missionCompleteTimer = 0.0f;
                }
                else
                {
                    missionComplete = false;
                    missionCompleteStep = 0;
                    missionCompleteTimer = 0.0f;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        primObjNeed = 1;
        secObjNeed = 2;
        primObjCollect = 0;
        secObjCollect = 0;
        missionTimer = 0.0f;

        primObjScore = 0;
        secObjScore = 0; 
        timeScore = 0; 
        totalScore = 0;
        totalGrade = "F";
        collection = GameObject.FindGameObjectsWithTag("Collectable");
        mainMenu.SetActive(true);
        gameInfoMenu.SetActive(false);
        missionDebriefMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        missionCompleteMenu.SetActive(false);
        playing = false;
        player.PausePlayer(true);
        foreach (EnemyController enemy in enemies)
        {
            enemy.PauseGame(true);
        }
    }

    public void StartMission()
    {
        foreach (GameObject obj in collection)
        {
            obj.GetComponent<Collectable>().ResetCollectable();
        }
        missionTimerVal.GetComponent<TMPro.TextMeshProUGUI>().text = "00:00";
        missionPrimObjVal.GetComponent<TMPro.TextMeshProUGUI>().text = "0 / 1";
        missionSecObjVal.GetComponent<TMPro.TextMeshProUGUI>().text = "0 / 2";
        missionTimerScore.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        missionPrimObjScore.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        missionSecObjScore.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        missionScore.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        missionGrade.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        player.PausePlayer(false);
        player.ResetPosition();
        foreach (EnemyController enemy in enemies)
        {
            enemy.PauseGame(false);
            enemy.RestartGame();
        }
        missionDebriefMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(false);
        paused = false;
        playing = true;

        primObjNeed = 1;
        secObjNeed = 2;
        primObjCollect = 0;
        secObjCollect = 0;
        missionTimer = 0.0f;

        primObjScore = 0;
        secObjScore = 0;
        timeScore = 0;
        totalScore = 0;
        totalGrade = "F";

        hudPrimObj.GetComponent<TMPro.TextMeshProUGUI>().text = primObjCollect.ToString() + " / " + primObjNeed.ToString();
        hudSecObj.GetComponent<TMPro.TextMeshProUGUI>().text = secObjCollect.ToString() + " / " + secObjNeed.ToString();
    }

    public void MissionDebrief()
    {
        missionDebriefMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        gameInfoMenu.SetActive(false);
        missionDebriefMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        missionCompleteMenu.SetActive(false);
    }

    public void RestartMission()
    {
        player.ResetPosition();
        foreach (EnemyController enemy in enemies)
        {
            enemy.RestartGame();
        }
        StartMission();
    }

    public void CompleteMission()
    {
        if (secObjCollect > 0)
        {
            secObjScore = (secObjCollect / secObjNeed) * 3000;
        }
        if (primObjCollect > 0)
        {
            primObjScore = (primObjCollect / primObjNeed) * 5000;
        }
        if (missionTimer < 690.0f)
        {
            timeScore = 2300 - (int)(missionTimer / 0.3f);
        }
        totalScore = secObjScore + primObjScore + timeScore;
        if (totalScore <= 6500)
        {
            totalGrade = "D";
        }
        else if (totalScore <= 8000)
        {
            totalGrade = "C";
        }
        else if (totalScore <= 9000)
        {
            totalGrade = "B";
        }
        else if (totalScore <= 10000)
        {
            totalGrade = "A";
        }
        else if(totalScore > 10000)
        {
            totalGrade = "S";
        }
        missionCompleteMenu.SetActive(true);
        missionComplete = true;
    }

    public void EndMission()
    {
        // failed
        gameOverMenu.SetActive(true);
    }

    public void PauseGame()
    {
        if(!paused && playing)
        {
            player.PausePlayer(true);
            foreach (EnemyController enemy in enemies)
            {
                enemy.PauseGame(true);
            }
            pauseMenu.SetActive(true);
            paused = true;
        }
        else if (playing)
        {
            player.PausePlayer(false);
            foreach (EnemyController enemy in enemies)
            {
                enemy.PauseGame(false);
            }
            pauseMenu.SetActive(false);
            paused = false;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenGuide(string menuName)
    {
        lastMenu = menuName;
        mainMenu.SetActive(false);
        gameInfoMenu.SetActive(true);
        missionDebriefMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        missionCompleteMenu.SetActive(false);
    }

    public void CloseGuide()
    {
        gameInfoMenu.SetActive(false);
        switch (lastMenu)
        {
            case "":
                mainMenu.SetActive(true);
                break;
            case "MainMenu":
                mainMenu.SetActive(true);
                break;
            case "PauseMenu":
                pauseMenu.SetActive(true);
                break;
            case "GameOverMenu":
                gameOverMenu.SetActive(true);
                break;
        }
    }

    void UpdateTimer()
    {
        hudTimer.GetComponent<TMPro.TextMeshProUGUI>().text = GetTimeString(true);
    }

    void UpdateObjective(int objective, int val)
    {
        if (objective == 0)
        {
            // main obj
        }
        else if (objective == 2)
        {
            // secondary obj
        }
    }

    void UpdateMissionEnd(int textNum)
    {
        switch (textNum)
        {
            case 0:
                missionPrimObjVal.GetComponent<TMPro.TextMeshProUGUI>().text = primObjCollect.ToString() + " / " + primObjNeed.ToString();
                break; 
            case 1:
                missionPrimObjScore.GetComponent<TMPro.TextMeshProUGUI>().text = primObjScore.ToString();
                break; 
            case 2:
                missionSecObjVal.GetComponent<TMPro.TextMeshProUGUI>().text = secObjCollect.ToString() + " / " + secObjNeed.ToString();
                break;
            case 3:
                missionSecObjScore.GetComponent<TMPro.TextMeshProUGUI>().text = secObjScore.ToString();
                break;
            case 4:
                missionTimerVal.GetComponent<TMPro.TextMeshProUGUI>().text = GetTimeString(false);
                break;
            case 5:
                missionTimerScore.GetComponent<TMPro.TextMeshProUGUI>().text = missionTimerScore.ToString();
                break;
            case 6:
                missionScore.GetComponent<TMPro.TextMeshProUGUI>().text = totalScore.ToString();
                break;
            case 7:
                missionGrade.GetComponent<TMPro.TextMeshProUGUI>().text = totalGrade;
                break;
        }
    }

    string GetTimeString(bool hudTime)
    {
        int totalSeconds = (int)(missionTimer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds - minutes * 60;
        int milSeconds = (int)((missionTimer - totalSeconds)*100);

        string secText;
        string milSecText;
        if (seconds < 10)
        {
            secText = "0" + seconds.ToString();
        }
        else
        {
            secText = seconds.ToString();
        }
        if (milSeconds < 10)
        {
            milSecText = "0" + milSeconds.ToString();
        }
        else
        {
            milSecText = milSeconds.ToString();
        }

        if (hudTime)
        {
            return new string(minutes.ToString() + ":" + secText + ":" + milSecText);
        }
        else
        {
            return new string(minutes.ToString() + ":" + secText);
        }
    }

    public void TryToCatch()
    {
        if (!playerIsSafe)
        {
            // gameover
            PauseGame();
            pauseMenu.SetActive(false);
            EndMission();
        }
    }

    public void SetPlayerSafe(bool safe)
    {
        playerIsSafe = safe;
        if (playerIsSafe && primObjCollect == primObjNeed)
        {
            player.PausePlayer(true);
            foreach (EnemyController enemy in enemies)
            {
                enemy.PauseGame(true);
            }
            CompleteMission();
        }
    }

    public void CollectObj(int objType)
    {
        if (objType == 1)
        {
            primObjCollect++;
            hudPrimObj.GetComponent<TMPro.TextMeshProUGUI>().text = primObjCollect.ToString() + " / " + primObjNeed.ToString();
        }
        else if (objType == 2)
        {
            secObjCollect++;
            hudSecObj.GetComponent<TMPro.TextMeshProUGUI>().text = secObjCollect.ToString() + " / " + secObjNeed.ToString();
        }
    }

    public void TryPause()
    {
        if (!paused && playing)
        {
            PauseGame();
        }
    }
}
