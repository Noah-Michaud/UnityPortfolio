using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class ExplosionController : MonoBehaviour
{
    public bool fired = false;
    public float damage = 1.0f;
    float lifeTimer = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }

    }

    public void FireCannon(float dmg, float range)
    {
        damage = dmg;
        fired = true;
        lifeTimer = 0.5f;
        this.gameObject.transform.localScale = new Vector3(range,range,range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage, true);

        }
    }


}
