using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public bool playing = false;
    //public ArrowController arrowCont;
    public GameObject arrowPrefab;
    GameObject currentArrow;
    //public float power = 10f;
    public bool canFire = true;
    public GameObject bow;
    public SpriteRenderer arrowSprite;
    public SpriteRenderer handSprite;

    GameObject[] firedArrows;
    int numOfArrows = 0;

    float reloadTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        firedArrows = new GameObject[50];
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetMouseButton(0) && canFire == true)
        {
            FireArrow(new Vector2(0.5f, 0.5f), power);
            canFire = false;
        }
        */
        if (canFire == false)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0.0f)
            {
                canFire = true;
                arrowSprite.enabled = true;
                handSprite.enabled = true;
            }
        }
    }

    public void TryFire(Vector2 direction, float power)
    {
        if (canFire == true)
        {
            FireArrow(direction, power);
        }
    }

    public void FireArrow(Vector2 direction, float power)
    {
        currentArrow = Instantiate(arrowPrefab, this.gameObject.transform.position, Quaternion.identity);
        currentArrow.GetComponent<ArrowController>().player = this;
        Rigidbody arrowRB = currentArrow.GetComponent<ArrowController>().rb;
        arrowRB.transform.rotation = Quaternion.Euler(direction.x, direction.y, 0);
        arrowRB.velocity = new Vector3(power * direction.x, power * direction.y, 0);
        currentArrow.GetComponent<ArrowController>().hasFired = true;
        canFire = false;
        arrowSprite.enabled = false;
        handSprite.enabled = false;
        reloadTimer = 1.0f;
        firedArrows[numOfArrows] = currentArrow;
        numOfArrows++;
    }

    public void ArrowHit(string hit)
    {
        //canFire = true;
        
        gameController.HitObject(hit);
    }

    public void UpdateBow(float angle)
    {
        bow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void DestroyArrows()
    {
        for (int i = 0; i < numOfArrows; i++)
        {
            Destroy(firedArrows[i]);
        }
        numOfArrows = 0;
    }
}
