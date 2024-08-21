using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class EnemySpawner : MonoBehaviour
{
    public GameController game;
    public int[] pathPoints = new int[30];
    Dictionary<int, GameObject> points = new Dictionary<int, GameObject>();

    public GameObject enemyPrefabLight;
    public GameObject enemyPrefabMedium;
    public GameObject enemyPrefabHeavy;
    public GameObject prefabBlobert;
    bool spawning = false;

    int enemyNum = 10;
    float nextEnemySpawn = 0.0f;
    float spawnTimer = 0.0f;
    int currentSpawnNum = 0;

    float lightEnemyChance = 1.0f;
    float mediumEnemyChance = 0.0f;
    float heavyEnemyChance = 0.0f;

    int roundnum = 0;

    bool blobertHasSpawned = false;
    bool paused = false;
    public enum EnemyTypes
    {
        Light,
        Medium,
        Heavy,
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] foundPoints = GameObject.FindGameObjectsWithTag("Point");
        for (int i = 0; i < foundPoints.Length; i++)
        {
            points.Add(foundPoints[i].GetComponent<Points>().pointNum, foundPoints[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning && !paused)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= nextEnemySpawn)
            {
                spawnTimer = 0.0f;
                SpawnEnemy();
                currentSpawnNum++;

            }
            if (currentSpawnNum >= enemyNum)
            {
                spawning = false;
                game.StoppedSpawning();
            }
        }
    }

    public void PauseGame(bool gameState)
    {
        paused = gameState;
    }

    public void StartSpawning(int numOfEnemy, float timeBetweenSpawns, float lightChance, float medChance, float heavyChance, int roundNum)
    {
        roundnum= roundNum;
        enemyNum = numOfEnemy;
        nextEnemySpawn = timeBetweenSpawns;
        spawnTimer = 0.0f;
        currentSpawnNum = 0;

        lightEnemyChance = lightChance;
        mediumEnemyChance = medChance;
        heavyEnemyChance = heavyChance;

        spawning = true;
    }

    void SpawnEnemy()
    {
        float blobingTime = 0.0f;
        if (!blobertHasSpawned)
        {
            blobingTime = (float)(roundnum) * 0.001f;
        }
        float rand = Random.Range(0.0f, 1.0f + blobingTime);
        if (rand <= lightEnemyChance)
        {
            SpawnEnemyType(EnemyTypes.Light);
        }
        else if (rand <= lightEnemyChance + mediumEnemyChance)
        {
            SpawnEnemyType(EnemyTypes.Medium);
        }
        else if (rand <= lightEnemyChance + mediumEnemyChance + heavyEnemyChance)
        {
            SpawnEnemyType(EnemyTypes.Heavy);
        }
        else
        {
            SpawnBlobert();
        }

    }

    public void SpawnEnemyType(EnemyTypes type)
    {
        GameObject newEnemy = null;
        switch (type)
        {
            case EnemyTypes.Light:
                newEnemy = Instantiate(enemyPrefabLight, this.gameObject.transform.position, Quaternion.identity); 
                break; 
            case EnemyTypes.Medium:
                newEnemy = Instantiate(enemyPrefabMedium, this.gameObject.transform.position, Quaternion.identity);
                break; 
            case EnemyTypes.Heavy:
                newEnemy = Instantiate(enemyPrefabHeavy, this.gameObject.transform.position, Quaternion.identity);
                break;
        }
        if (newEnemy != null)
        {
            newEnemy.GetComponent<EnemyController>().points = points;
            newEnemy.GetComponent<EnemyController>().pathPoints = pathPoints;
            newEnemy.GetComponent<EnemyController>().PauseGame(false);
            game.AddEnemy(newEnemy);
        }
    }

    public void SpawnBlobert()
    {
        blobertHasSpawned = true;
        GameObject newEnemy = Instantiate(prefabBlobert, new Vector3(this.gameObject.transform.position.x - 17, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        newEnemy.GetComponent<EnemyController>().points = points;
        newEnemy.GetComponent<EnemyController>().pathPoints = pathPoints;
        newEnemy.GetComponent<EnemyController>().PauseGame(false);
        game.AddEnemy(newEnemy);
        game.BlobertHasSpawned();
    }
    public void RestartGame()
    {
        spawning = false;

        enemyNum = 10;
        nextEnemySpawn = 0.0f;
        spawnTimer = 0.0f;
        currentSpawnNum = 0;

        lightEnemyChance = 1.0f;
        mediumEnemyChance = 0.0f;
        heavyEnemyChance = 0.0f;

        roundnum = 0;

        blobertHasSpawned = false;
        paused = false;
    }
}
