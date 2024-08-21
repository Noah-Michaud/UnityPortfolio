using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameController game;

    public float speed = 1.0f;
    public int numOfPoints;
    public int numOfStops;
    public int[] pathPoints = new int[30];
    public int[] stopPoints = new int[10];
    public int[] hallwayPath = new int[10];
    public int[] returnPath = new int[10];
    Dictionary<int, GameObject> points = new Dictionary<int, GameObject>();

    Vector3 chasePoint;

    Rigidbody rb;
    public int currentPoint = 0;
    public int heading = 0;
    Vector3 facing = new Vector3(0,0,0);
    bool chasing = false;
    bool returning = false;
    Vector3 currentPos;

    bool paused = false;

    bool check = false;
    float checkTimer = 0.0f;
    Vector3 nextPos;
    int nextPoint;
    float currentAngle;

    public bool playerIsSafe = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] foundPoints = GameObject.FindGameObjectsWithTag("Point");
        for (int i = 0; i< foundPoints.Length; i++)
        {
            points.Add(foundPoints[i].GetComponent<Points>().pointNum, foundPoints[i]);
        }
        rb = GetComponent<Rigidbody>();
        gameObject.transform.position = points[pathPoints[0]].transform.position;
        currentPoint = 0;
        heading = pathPoints[1];
    }

    public void RestartGame()
    {
        gameObject.transform.position = points[pathPoints[0]].transform.position;
        currentPoint = 0;
        chasing = false;
        check = false;
        FaceDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (chasing == false && returning == false && check == false)
            {
                if (currentPoint + 1 >= numOfPoints)
                {
                    //gameObject.transform.position += (points[pathPoints[0]].transform.position - points[pathPoints[currentPoint]].transform.position) * Time.deltaTime * speed;
                    rb.velocity = Vector3.Normalize(points[pathPoints[0]].transform.position - points[pathPoints[currentPoint]].transform.position) * speed;
                    if (Vector3.Distance(gameObject.transform.position, points[pathPoints[0]].transform.position) < 0.1f)
                    {
                        for (int i = 0; i < stopPoints.Length; i++)
                        {
                            if (stopPoints[i] == pathPoints[0])
                            {
                                //check = true;
                                i = stopPoints.Length;
                                StartCheck(points[pathPoints[0]].transform.position, 0);
                            }
                        }
                        if (!check)
                        {
                            gameObject.transform.position = points[pathPoints[0]].transform.position;
                            currentPoint = 0;
                            FaceDirection();
                            //heading = pathPoints[1];
                        }


                    }
                }
                else
                {
                    //gameObject.transform.position += (points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * Time.deltaTime * speed;
                    rb.velocity = Vector3.Normalize(points[pathPoints[currentPoint + 1]].transform.position - points[pathPoints[currentPoint]].transform.position) * speed;
                    if (Vector3.Distance(gameObject.transform.position, points[pathPoints[currentPoint + 1]].transform.position) < 0.1f)
                    {
                        for (int i = 0; i < stopPoints.Length; i++)
                        {
                            if (stopPoints[i] == pathPoints[currentPoint + 1])
                            {
                                //check = true;
                                i = stopPoints.Length;
                                StartCheck(points[pathPoints[currentPoint + 1]].transform.position, currentPoint + 1);
                            }
                        }
                        if (!check)
                        {
                            gameObject.transform.position = points[pathPoints[currentPoint + 1]].transform.position;
                            currentPoint += 1;
                            FaceDirection();
                            //heading = pathPoints[currentPoint + 1];
                        }



                    }
                }
                RayCast(currentAngle);
                if (chasing) // this can be removed if i try something else
                {
                    game.TryToCatch();
                }
            }
            else if (chasing)
            {
                // ------------------------------ Change to find nearest "hallway" point, follow hallway
                // return to nearest hallway path then follow to elevator
                // if player is safe when reaching 0, return to normal path
                // else, game over
            }
            else if (returning)
            {

            }
            else if (check)
            {
                if (checkTimer < 2.0f)
                {

                    gameObject.transform.rotation = Quaternion.Euler(0, (currentAngle - 90.0f) + (90 * checkTimer), 0);
                    RayCast((currentAngle - 90.0f) + (90 * checkTimer));
                    checkTimer += Time.deltaTime;
                    if (chasing)
                    {
                        game.TryToCatch();
                    }
                }
                else
                {
                    EndCheck();
                }
            }
        }
        
    }

    public void StartChase(Vector3 seen)
    {
        chasing = true;
        chasePoint = seen;
        currentPos = gameObject.transform.position;
    }

    void RayCast(float inputAngle)
    {
        for (int i = 0; i < 7; i++)
        {
            float cos = Mathf.Cos(((inputAngle - 12.0f) + (4.0f * i))*Mathf.Deg2Rad);
            float sin = Mathf.Sin(((inputAngle - 12.0f) + (4.0f * i))* Mathf.Deg2Rad);

            Vector3 angleOfRay = new Vector3(cos, 0, -sin);

            RaycastHit hit;
            Physics.Raycast(new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z), angleOfRay, out hit, 10.0f);
            //Debug.DrawRay(new Vector3(gameObject.transform.position.x, 0.1f, gameObject.transform.position.z), angleOfRay * hit.distance, Color.red, 0.05f);
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.tag + "RC Hit");
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    i = 10;
                    chasePoint = new Vector3(hit.collider.gameObject.transform.position.x, 0, hit.collider.gameObject.transform.position.z);
                    chasing = true;
                    Debug.Log("Saw Player");
                }
            }
            else
            {
                //Debug.Log("wat");
            }


        }

    }

    void FaceDirection()
    {
        float directX = 0.0f;
        float directZ = 0.0f;
        if (chasing)
        {
            directX = chasePoint.x - points[pathPoints[currentPoint]].transform.position.x;
            directZ = chasePoint.z - points[pathPoints[currentPoint]].transform.position.z;
        }
        else if (currentPoint + 1 >= numOfPoints)
        {
            directX = points[pathPoints[0]].transform.position.x - points[pathPoints[currentPoint]].transform.position.x;
            directZ = points[pathPoints[0]].transform.position.z - points[pathPoints[currentPoint]].transform.position.z;
        }
        else
        {
            directX = points[pathPoints[currentPoint + 1]].transform.position.x - points[pathPoints[currentPoint]].transform.position.x;
            directZ = points[pathPoints[currentPoint + 1]].transform.position.z - points[pathPoints[currentPoint]].transform.position.z;
        }

        facing = Vector3.Normalize(new Vector3(directX, 0.0f, directZ));
        float rot = Mathf.Acos(facing.x)*Mathf.Rad2Deg;
        if (facing.z > 0.01f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -rot, 0);
            currentAngle = -rot;
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, rot, 0);
            currentAngle = rot;
        }
        
    }

    void StartCheck(Vector3 next, int thenextPoint)
    {
        nextPos = next;
        nextPoint = thenextPoint;
        rb.velocity = Vector3.zero;
        check = true;
    }

    void EndCheck()
    {
        gameObject.transform.position = nextPos;
        currentPoint = nextPoint;
        FaceDirection();
        heading = pathPoints[nextPoint];
        check = false;
        checkTimer = 0.0f;
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
        if (other.gameObject.CompareTag("Player"))
        {
            game.TryToCatch();
        }
    }
}
