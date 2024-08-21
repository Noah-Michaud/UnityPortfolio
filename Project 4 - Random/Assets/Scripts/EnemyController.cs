using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class EnemyController : MonoBehaviour
{
    public GameController game;

    public float speed = 1.0f;
    public float maxHealth = 10.0f;
    public int baseDamage = 1;
    public bool armored = false;
    public int gold = 5;


    public Dictionary<int, GameObject> points = new Dictionary<int, GameObject>();
    public int[] pathPoints = new int[30];

    public bool isBlobert = false;

    Rigidbody rb;
    int currentPoint = 0;

    bool paused = true;

    bool slowed = false;
    float slowTimer = 0.0f;
    float slowSpeed = 1.0f;

    bool blobertReached = false;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        //gameObject.transform.position = points[pathPoints[0]].transform.position;
        currentPoint = 0;
    }

    public void RestartGame()
    {
        gameObject.transform.position = points[pathPoints[0]].transform.position;
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (slowed)
            {
                slowTimer -= Time.deltaTime;
                if (slowTimer <= 0.0f)
                {
                    slowed = false;
                }
            }
            if (currentPoint + 2 == pathPoints.Length)
            {
                // --------- Change this to doing damage to base
                if (slowed)
                {
                    rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * slowSpeed;
                }
                else
                {
                    rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * speed;
                }
                
                if (Vector3.Distance(gameObject.transform.position, points[pathPoints[currentPoint + 1]].transform.position) < 0.1f)
                {
                    game.DamageBase(this);
                    //game.RemoveEnemy(this.gameObject);
                }
            }
            else
            {
                if (slowed)
                {
                    rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * slowSpeed;
                }
                else
                {
                    rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * speed;
                }
                //rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * speed;
                if (Vector3.Distance(gameObject.transform.position, points[pathPoints[currentPoint + 1]].transform.position) < 0.1f)
                {
                    gameObject.transform.position = points[pathPoints[currentPoint + 1]].transform.position;
                    currentPoint += 1;
                }
            }
            
        }

    }

    public void TakeDamage(float damage, bool pen)
    {
        if (armored && !pen)
        {
            game.TryDamage(this.gameObject, damage / 2.0f);
            //maxHealth -= damage/2.0f;
        }
        else
        {
            game.TryDamage(this.gameObject, damage);
            //maxHealth -= damage;
        }

        /*
        if(maxHealth <= 0.0f)
        {
            Debug.Log("died");
            game.RemoveEnemy(this.gameObject);
        }
        */
    }

    public bool UpdateHealth(float dmg)
    {
        maxHealth -= dmg;
        if (maxHealth < 0.0f)
        {
            return true;
        }
        return false;
    }

    public void Slow(float timeSlowed, float amount)
    {
        slowed = true;
        slowTimer = timeSlowed;
        slowSpeed = speed * amount;
    }

    public void PauseGame(bool gameState)
    {
        if (gameState)
        {
            paused = true;
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
        else
        {
            paused = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            other.gameObject.GetComponent<TowerController>().AddTarget(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            other.gameObject.GetComponent<TowerController>().RemoveTarget(this.gameObject);
        }
    }
}
