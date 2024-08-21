using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Vector3 originalPos;
    public GameController game;
    public int collectType = 0;
    bool pickedUp = false;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && pickedUp == false)
        {
            game.CollectObj(collectType);
            pickedUp = true;
            gameObject.transform.position = new Vector3(-10,-10,-10);
        }
    }

    public void ResetCollectable()
    {
        gameObject.transform.position = originalPos;
        pickedUp = false;
    }
}
