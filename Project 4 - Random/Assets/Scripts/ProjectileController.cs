using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class ProjectileController : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 velocity;
    public bool fired = false;
    public float damage = 1.0f;
    public bool armorPen = false;
    float lifeTimer = 5.0f;
    int remaining = 1;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (fired)
        {
            this.gameObject.transform.position += velocity * Time.deltaTime;
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    public void FireProjectile(Vector3 vel, float dmg, bool pen, int numOfHits, float lifetime)
    {
        velocity = vel;
        damage = dmg;
        armorPen = pen;
        remaining = numOfHits;
        fired = true;
        lifeTimer = lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage, armorPen);
            remaining--;
            if (remaining <=0)
            {
                Destroy(this.gameObject);
            }
            
        }
        if (other.gameObject.CompareTag("OOB"))
        {
            Destroy(this.gameObject);

        }
    }
}
