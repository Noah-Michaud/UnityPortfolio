using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHoldSpwner : MonoBehaviour
{
    public GameObject handHoldPrefab;

    Queue<GameObject> handHolds;

    public int numberOfLevels = 0;
    public float vertSpace = 0.0f;
    public float horizSpace = 0.0f;
    public float vertRandom = 0.0f;
    public float horizRandom = 0.0f;
    public float width = 0.0f;
    public float height = 0.0f;
    public float start = 0.0f;

    float xGrowth = 0.0f;
    float yGrowth = 0.0f;

    public float currentHeight = 0.0f;
    int wide;

    public float playerPos;

    bool playing = false;
    // Start is called before the first frame update

    private void Start()
    {
        handHolds = new Queue<GameObject>();
        xGrowth = 0.0f;
        yGrowth = 0.0f;
        playerPos = 2.4f;
    }
    public void BuildHolds()
    {
        
        //wide = (int)(width / (horizSpace + xGrowth));
        for (int i = 0; i < numberOfLevels; i++)
        {
            SpawnHolds();
            /*
            for (int k = 0; k < wide; k++)
            {
                float xVal = -(width / 2.0f) + ((horizSpace + xGrowth) * (float)k) + ((horizSpace + xGrowth) / 2.0f) + UnityEngine.Random.Range(-horizRandom, horizRandom);
                float yVal = start + ((vertSpace + yGrowth) * i) + UnityEngine.Random.Range(-vertRandom, vertRandom);
                GameObject currentHold = Instantiate(handHoldPrefab, this.gameObject.transform.position, Quaternion.identity);
                currentHold.transform.position = new Vector3(xVal, yVal, 1);
                handHolds.Enqueue(currentHold);
            }
            yGrowth += 0.005f;
            xGrowth += 0.005f;
            */
        }
        //currentHeight = start + (vertSpace * numberOfLevels);
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos + 20.0f > currentHeight && playing)
        {
            RemoveHolds();
            Debug.Log("moving holds");
        }
    }

    void RemoveHolds()
    {
        // remove bottom layer of holds
        for (int i = 0; i < wide; i++)
        {
            GameObject currentHold = handHolds.Dequeue();
            Destroy(currentHold);
        }
        SpawnHolds();
    }

    public void SpawnHolds()
    {
        // spawn layer of holds
        wide = (int)(width / (horizSpace + xGrowth));
        for (int i = 0; i < wide; i++)
        {
            float xVal = -(width / 2.0f) + ((horizSpace + xGrowth) * (float)i) + ((horizSpace + xGrowth) / 2.0f) + UnityEngine.Random.Range(-horizRandom, horizRandom);
            float yVal = currentHeight + UnityEngine.Random.Range(-vertRandom, vertRandom);
            GameObject currentHold = Instantiate(handHoldPrefab, this.gameObject.transform.position, Quaternion.identity);
            currentHold.transform.position = new Vector3(xVal, yVal, 1);
            handHolds.Enqueue(currentHold);
        }
        currentHeight += (vertSpace + yGrowth);
        yGrowth += 0.005f;
        xGrowth += 0.005f;
    }

    public void UpdatePlayerPos(float pos)
    {
        playerPos = pos;
    }

    public void ResetHolds()
    {
        playing = false;
        while (handHolds.Count > 0)
        {
            GameObject currentHold = handHolds.Dequeue();
            Destroy(currentHold);
        }
        xGrowth = 0.0f;
        yGrowth = 0.0f;
        currentHeight = 0.0f;
        playerPos = 2.4f;
        

        BuildHolds();
    }

}
