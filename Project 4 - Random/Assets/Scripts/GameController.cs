using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class GameController : MonoBehaviour
{
    public TowerManager towerManager;
    public EnemySpawner enemySpawner;
    public int roundNum = 1;
    public int initialEnemyNum = 10;
    public int enemyGrowth = 5;
    public float timeToSpawn = 10.0f;
    public int baseMaxHealth = 200;
    int baseCurrentHealth = 200;
    int enemyNum = 10;
    float nextEnemySpawn = 0.0f;
    float spawnTimer = 0.0f;
    List<GameObject> enemyHolder = new List<GameObject>();
    //List<GameObject> towerHolder = new List<GameObject>();

    int currency = 300;
    bool isBuyingTower = false;
    TowerController.TowerType buyingType = TowerController.TowerType.None;
    int currentTowerSelected = -1;

    Dictionary<GameObject, float> frameDamage = new Dictionary<GameObject, float>();
    List<GameObject> enemiesToDestoy = new List<GameObject>();
    bool spawning = false;
    int currentSpawnNum = 0;

    float lightEnemyChance = 1.0f;
    float mediumEnemyChance = 0.0f;
    float heavyEnemyChance = 0.0f;

    GameObject[] towerHolder = new GameObject[17];
    int[,] map = new int[4, 9] { {  0,-1,-1,-1, 0,-1,-1,-1, 0 },
                                 {  0,-1, 0,-1, 0,-1, 0,-1, 0 },
                                 { -1,-1, 0,-1, 0,-1, 0,-1,-1 },
                                 {  0, 0, 0,-1,-1,-1, 0, 0, 0 } };

    public GameObject roundCounter;
    public GameObject currencyText;
    public GameObject baseHealthText;
    public GameObject editPanel;
    public GameObject towerInfoPanel;
    public GameObject towerType;
    public GameObject towerSubType;
    public GameObject towerLevel;
    public GameObject towerAtkSpeed;
    public GameObject towerAtkDmg;
    public GameObject towerArmPen;
    public GameObject towerTypes1;
    public GameObject towerTypes2;
    public GameObject towerTypes3;
    public GameObject rerollText;
    public GameObject nextRoundText;

    public GameObject blobertPanel;
    public GameObject blobertHealth;

    public GameObject mainMenuPanel;
    public GameObject infoMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;

    bool paused = false;

    bool editing = false;
    int rerolls = 1;

    bool blobertHasSpawned = false;
    bool showBlobertBar = false;
    float blobertTime = 16.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyNum = initialEnemyNum;
        roundCounter.GetComponent<TMPro.TextMeshProUGUI>().text = roundNum.ToString();
        currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
        baseHealthText.GetComponent<TMPro.TextMeshProUGUI>().text = baseCurrentHealth.ToString();
        mainMenuPanel.SetActive(true);
        infoMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (!spawning)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer > 1.0f)
                {
                    //StartNewRound();
                    //spawning = true;
                }
            }
            else if (!blobertHasSpawned)
            {
                if(Input.GetKey(KeyCode.B))
                {
                    enemySpawner.SpawnBlobert();
                    blobertHasSpawned = true;
                }
            }
            if (blobertHasSpawned && !showBlobertBar)
            {
                blobertTime -= Time.deltaTime;
                if (blobertTime <= 0.0f)
                {
                    showBlobertBar = true;
                    ShowBlobert();
                }
            }

            // calculate
            DealDamage();
            frameDamage.Clear();
        }
        
    }

    public void StartRound()
    {
        if (!spawning && enemyHolder.Count == 0)
        {
            StartNewRound();
            spawning = true;
            nextRoundText.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
        }
    }

    public void BuyTower(int type)
    {
        switch (type)
        {
            case 0:
                Debug.Log("trying to buy archer");
                BuyingTower(TowerController.TowerType.Archer);
                
                break;
            case 1:
                Debug.Log("trying to buy fighter");
                BuyingTower(TowerController.TowerType.Fighter);
                
                break; 
            case 2:
                Debug.Log("trying to buy cannon");
                BuyingTower(TowerController.TowerType.Cannon);
                
                break;
        }
    }

    public void SellTower()
    {
        if (editing && currentTowerSelected >= 0)
        {
            if (towerHolder[currentTowerSelected] != null)
            {
                currency += 50 * towerHolder[currentTowerSelected].gameObject.GetComponent<TowerController>().level;
                GameObject toRemove = towerHolder[currentTowerSelected];
                towerHolder[currentTowerSelected] = null;
                Destroy(toRemove);
                currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
                CancelEdit();
            }
            
        }
    }

    public void BuyingTower(TowerController.TowerType towerType)
    {
        switch (towerType)
        {
            case TowerController.TowerType.Archer:
                if (currency >= 100)
                {
                    isBuyingTower = true;
                    buyingType = TowerController.TowerType.Archer;
                    Debug.Log("trying next to buy archer");
                }
                break;
            case TowerController.TowerType.Cannon:
                if (currency >= 100)
                {
                    isBuyingTower = true;
                    buyingType = TowerController.TowerType.Cannon;
                    Debug.Log("trying next to buy cannon");
                }
                break;
            case TowerController.TowerType.Fighter:
                if (currency >= 100)
                {
                    isBuyingTower = true;
                    buyingType = TowerController.TowerType.Fighter;
                    Debug.Log("trying next to buy fighter");
                }
                break;
        }

        if (isBuyingTower)
        {
            StartEdit();
        }
    }

    public void CancelEdit()
    {
        isBuyingTower = false;
        buyingType = TowerController.TowerType.None;
        editPanel.SetActive(false);
        towerInfoPanel.SetActive(false);
        editing = false;
        currentTowerSelected = -1;
    }

    public void StartEdit()
    {
        editing = true;
        editPanel.SetActive(true);
    }

    public void EditTower(int towerSpot)
    {
        if (isBuyingTower)
        {
            if (towerHolder[towerSpot] == null)
            {
                TryPlaceTower(towerSpot);
            }
        }
        else
        {
            if (towerHolder[towerSpot] != null)
            {
                currentTowerSelected = towerSpot;
                // open screen for reroll or upgrade
                towerInfoPanel.SetActive(true);
                //towerInfoPanel
                switch (towerHolder[towerSpot].gameObject.GetComponent<TowerController>().towerType)
                {
                    case TowerController.TowerType.Archer:
                        towerType.GetComponent<TMPro.TextMeshProUGUI>().text = "Archer";
                        break;
                    case TowerController.TowerType.Cannon:
                        towerType.GetComponent<TMPro.TextMeshProUGUI>().text = "Cannon";
                        break;
                    case TowerController.TowerType.Fighter:
                        towerType.GetComponent<TMPro.TextMeshProUGUI>().text = "Fighter";
                        break;
                }
                
                towerSubType.GetComponent<TMPro.TextMeshProUGUI>().text = towerManager.GetSubtype(towerHolder[towerSpot].gameObject.GetComponent<TowerController>());
                towerLevel.GetComponent<TMPro.TextMeshProUGUI>().text = towerHolder[towerSpot].gameObject.GetComponent<TowerController>().level.ToString();
                towerAtkSpeed.GetComponent<TMPro.TextMeshProUGUI>().text = towerHolder[towerSpot].gameObject.GetComponent<TowerController>().GetAttackSpeed();
                towerAtkDmg.GetComponent<TMPro.TextMeshProUGUI>().text = towerHolder[towerSpot].gameObject.GetComponent<TowerController>().GetAttackDmg();
                if (towerHolder[towerSpot].gameObject.GetComponent<TowerController>().armorPen)
                {
                    towerArmPen.GetComponent<TMPro.TextMeshProUGUI>().text = "True";
                }
                else
                {
                    towerArmPen.GetComponent<TMPro.TextMeshProUGUI>().text = "false";
                }

                string[] data = towerManager.GetTowerInfoText(towerHolder[towerSpot].gameObject.GetComponent<TowerController>().towerType);
                
                towerTypes1.GetComponent<TMPro.TextMeshProUGUI>().text = data[0];
                towerTypes2.GetComponent<TMPro.TextMeshProUGUI>().text = data[1];
                towerTypes3.GetComponent<TMPro.TextMeshProUGUI>().text = data[2];
            }
        }
    }
    public void TryPlaceTower(int towerSpot)
    {
        float[] pos = TowerLocator(towerSpot);
        GameObject newTower = towerManager.PlaceTower(buyingType, new Vector3(pos[0], 0.0f, pos[1]));
        if (newTower != null)
        {
            currency -= 100;
            isBuyingTower = false;
            buyingType = TowerController.TowerType.None;
            currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
            towerHolder[towerSpot] = newTower;
            CancelEdit();
        }
    }

    public void RerollTower()
    {
        if (editing && currentTowerSelected >= 0 && rerolls == 1)
        {
            if (towerHolder[currentTowerSelected] != null)
            {
                towerHolder[currentTowerSelected] = towerManager.RerollTower(towerHolder[currentTowerSelected]);
                rerolls = 0;
                rerollText.GetComponent<TMPro.TextMeshProUGUI>().text = " ";
                CancelEdit();
            }

        }
    }

    public void UpgradeTower()
    {
        if (editing && currentTowerSelected >= 0)
        {
            if (towerHolder[currentTowerSelected] != null)
            {
                if (currency >= towerHolder[currentTowerSelected].GetComponent<TowerController>().level * 100)
                {
                    towerHolder[currentTowerSelected].GetComponent<TowerController>().LevelUp();
                    currency -= towerHolder[currentTowerSelected].GetComponent<TowerController>().level * 100;
                    currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
                    CancelEdit();
                }
                
            }

        }
    }

    float[] TowerLocator(int towerSpot)
    {
        float[] spot = new float[2];
        switch (towerSpot)
        {
            case 0: 
                spot = new float[2] { -12.0f, 4.5f };
                break;
            case 1:
                spot = new float[2] { -12.0f, 1.5f };
                break; 
            case 2:
                spot = new float[2] { -12.0f, -4.5f };
                break; 
            case 3:
                spot = new float[2] { -9.0f, -4.5f };
                break; 
            case 4:
                spot = new float[2] { -6.0f, -4.5f };
                break; 
            case 5:
                spot = new float[2] { -6.0f, -1.5f };
                break; 
            case 6:
                spot = new float[2] { -6.0f, 1.5f };
                break; 
            case 7:
                spot = new float[2] { 0.0f, 4.5f };
                break; 
            case 8:
                spot = new float[2] { 0.0f, 1.5f };
                break; 
            case 9:
                spot = new float[2] { 0.0f, -1.5f };
                break; 
            case 10:
                spot = new float[2] { 6.0f, 1.5f };
                break;
            case 11:
                spot = new float[2] { 6.0f, -1.5f };
                break;
            case 12:
                spot = new float[2] { 6.0f, -4.5f };
                break;
            case 13:
                spot = new float[2] { 9.0f, -4.5f };
                break;
            case 14:
                spot = new float[2] { 12.0f, 4.5f };
                break;
            case 15:
                spot = new float[2] { 12.0f, 1.5f };
                break;
            case 16:
                spot = new float[2] { 12.0f, 4.5f };
                break;
        }
        return spot;
    }
    void DealDamage()
    {
        if (frameDamage.Count > 0)
        {
            foreach (KeyValuePair<GameObject, float> pair in frameDamage)
            {
                if (pair.Key != null)
                {
                    
                    if (pair.Key.GetComponent<EnemyController>().UpdateHealth(pair.Value))
                    {
                        enemiesToDestoy.Add(pair.Key);
                    }
                    if (pair.Key.GetComponent<EnemyController>().isBlobert)
                    {
                        UpdateBlobertsHealthBar(pair.Key.GetComponent<EnemyController>().maxHealth);
                    }
                }
                
            }
            DestroyEnemies();
        }
    }

    void DestroyEnemies()
    {
        while(enemiesToDestoy.Count > 0)
        {
            if (enemiesToDestoy[0] != null)
            {
                
                GameObject enemy = enemiesToDestoy[0];
                currency += enemy.GetComponent<EnemyController>().gold;
                currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
                enemiesToDestoy.Remove(enemy);
                RemoveEnemy(enemy);
            }
            else
            {
                enemiesToDestoy.Remove(enemiesToDestoy[0]);
            }
        }
    }

    public void AddCurrency(int gold)
    {
        currency += gold;
        currencyText.GetComponent<TMPro.TextMeshProUGUI>().text = currency.ToString();
    }

    public void TryDamage(GameObject enemy, float damage)
    {
        if (frameDamage.ContainsKey(enemy))
        {
            frameDamage[enemy] += damage;
        }
        else
        {
            frameDamage.Add(enemy, damage);
        }
    }

    public void StartNewRound()
    {
        roundCounter.GetComponent<TMPro.TextMeshProUGUI>().text = roundNum.ToString();
        nextEnemySpawn = (float)(timeToSpawn / enemyNum);
        spawnTimer = 0.0f;
        if (roundNum < 5)
        {
            lightEnemyChance = 1.0f;
            mediumEnemyChance = 0.0f;
            heavyEnemyChance = 0.0f;
        }
        else if (roundNum < 10)
        {
            lightEnemyChance = 0.9f;
            mediumEnemyChance = 0.1f;
            heavyEnemyChance = 0.0f;
        }
        else if (roundNum < 20)
        {
            lightEnemyChance = 0.85f;
            mediumEnemyChance = 0.1f;
            heavyEnemyChance = 0.05f;
        }
        else if (roundNum < 50)
        {
            lightEnemyChance = 0.75f;
            mediumEnemyChance = 0.15f;
            heavyEnemyChance = 0.1f;
        }
        else
        {
            lightEnemyChance = 0.5f;
            mediumEnemyChance = 0.3f;
            heavyEnemyChance = 0.2f;
        }
        enemySpawner.StartSpawning(enemyNum, nextEnemySpawn, lightEnemyChance, mediumEnemyChance, heavyEnemyChance, roundNum);
        roundNum++;
        enemyNum += enemyGrowth;
        timeToSpawn += 1.0f;
    }


    public void DamageBase(EnemyController enemy)
    {
        baseCurrentHealth -= enemy.baseDamage;
        RemoveEnemy(enemy.gameObject);
        UpdateBaseHealth();
        if (baseCurrentHealth <= 0)
        {
            float breaker = 1.0f / 0.0f;
            Debug.Log(breaker);
        }
    }

    void UpdateBaseHealth()
    {
        baseHealthText.GetComponent<TMPro.TextMeshProUGUI>().text = baseCurrentHealth.ToString();
    }

    public void AddEnemy(GameObject enemy)
    {
        enemyHolder.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        //bool fine = false;
        enemyHolder.Remove(enemy);
        if (enemy.GetComponent<EnemyController>().isBlobert)
        {
            blobertPanel.SetActive(false);
        }
        /*
        foreach (GameObject tower in towerHolder)
        {
            if (tower.GetComponent<TowerController>().RemoveTarget(enemy))
            {
                fine = true;
                Debug.Log("It removed");
            }
            else
            {
                Debug.Log("It wont remove");
            }

        }
        if (fine)
        {
            Destroy(enemy);
        }
        */
        for (int i = 0; i < 17; i++)
        {
            if (towerHolder[i] != null)
            {
                towerHolder[i].GetComponent<TowerController>().RemoveTarget(enemy);
            }
        }
        currency += enemy.GetComponent<EnemyController>().gold;
        Destroy(enemy);

    }

    public void StoppedSpawning()
    {
        spawning = false;
        nextRoundText.GetComponent<TMPro.TextMeshProUGUI>().text = ">";
        if (rerolls == 0)
        {
            rerolls = 1;
            rerollText.GetComponent<TMPro.TextMeshProUGUI>().text = "?";
        }
    }

    public void BlobertHasSpawned()
    {
        blobertHasSpawned = true;

    }

    public void ShowBlobert()
    {
        blobertPanel.SetActive(true);
    }

    void UpdateBlobertsHealthBar(float health)
    {
        float perc = health / 5000.0f;
        blobertHealth.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(perc * 800.0f, 40.0f);
    }

    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
        infoMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void OpenInfoMenu()
    {
        mainMenuPanel.SetActive(false);
        infoMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenPauseMenu()
    {
        if (paused)
        {
            paused = false;
            enemySpawner.PauseGame(false);
            for (int i = 0; i < 17; i++)
            {
                if (towerHolder[i] != null)
                {
                    towerHolder[i].GetComponent<TowerController>().PauseGame(false);
                }
            }
            for (int i = 0; i < enemyHolder.Count; i++)
            {
                if (enemyHolder[i] != null)
                {
                    enemyHolder[i].GetComponent<TowerController>().PauseGame(false);
                }
            }
            mainMenuPanel.SetActive(false);
            infoMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }
        else
        {
            paused = true;
            enemySpawner.PauseGame(true);
            for (int i = 0; i < 17; i++)
            {
                if (towerHolder[i] != null)
                {
                    towerHolder[i].GetComponent<TowerController>().PauseGame(true);
                }
            }
            for (int i = 0; i < enemyHolder.Count; i++)
            {
                if (enemyHolder[i] != null)
                {
                    enemyHolder[i].GetComponent<TowerController>().PauseGame(true);
                }
            }
            mainMenuPanel.SetActive(false);
            infoMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }
        
    }

    void GameOver()
    {
        mainMenuPanel.SetActive(true);
        infoMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        for (int i = 0; i < 17; i++)
        {
            if (towerHolder[i] != null)
            {
                Destroy(towerHolder[i]);
            }
        }
        while (enemyHolder.Count > 0)
        {
            GameObject trash = enemyHolder[0];
            enemyHolder.RemoveAt(0);
            Destroy(trash);
        }
        roundNum = 1;
        timeToSpawn = 10.0f;
        baseCurrentHealth = 200;
        enemyNum = 10;
        nextEnemySpawn = 0.0f;
        spawnTimer = 0.0f;
        currency = 300;
        isBuyingTower = false;
        buyingType = TowerController.TowerType.None;
        currentTowerSelected = -1;
        spawning = false;
        currentSpawnNum = 0;
        lightEnemyChance = 1.0f;
        mediumEnemyChance = 0.0f;
        heavyEnemyChance = 0.0f;
        rerolls = 1;
        enemySpawner.RestartGame();
        mainMenuPanel.SetActive(false);
        infoMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
}
