using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class TowerController : MonoBehaviour
{
    // types
    // - archer (large range)
    //      -single, fast, med damage (recurve)
    //      -spread, med, low damage (tripple shot)
    //      -single, slow, med damage (long bow) (-penetrates)
    //
    // - melee (small range)
    //      -single, fast, med damage (spear)
    //      -aoe, med, med damage (sword slash)
    //      -single, med, low damage (brawler) (-slows)
    //
    // - Gunmen (medium range)
    //      -single, med, (single shot) (pen)
    //      -spread, med, (grapeshot) (pen)
    //      -aoe, slow, (explosive shot) (pen)

    // all
    public GameController game;
    public float fireSpeed = 1.0f;
    public float damage = 0.0f;
    public float range = 0.0f;
    public float projectileSpeed = 1.0f;
    public TowerType towerType;
    public DamageType damageType;
    public AttackSpeedType attackSpeedType;
    public bool armorPen = false;
    public float numOfShots = 2.0f;
    public float areaOfSpread = 30.0f;
    public float aoeArea = 0.0f;
    public int passThrough = 0;
    public GameObject projectile;
    public float projectileLife = 1.0f;

    public GameObject explosion;

    List<GameObject> enemiesToTarget = new List<GameObject>();
    public GameObject currentTarget = null;
    public GameObject exit;

    float damageBuff = 0.0f;
    float speedBuff = 0.0f;

    public int level = 1;
    public float levelBonus_attackDmg = 1.0f;
    public float levelBonus_attackSpeed = 0.2f;


    // archer
    // spread, num of projectiles

    // melee
    // aoe area, 

    // cannon

    float attackTime = 0.0f;
    float attackTimer = 0.0f;

    bool added = false;
    float adjustedPojSpeed = 1.0f;

    bool paused = false;

    public enum TowerType
    {
        Archer,
        Fighter,
        Cannon,
        None
    }

    public enum DamageType
    {
        Single,
        Spread,
        Aoe,
        Special,
    }

    public enum AttackSpeedType
    {
        Fast,
        Medium,
        Slow,
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (currentTarget != null)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackTime)
                {
                    Attack();
                    attackTimer = 0.0f;
                }
            }
        }
        
    }

    public void PauseGame(bool gameState)
    {
        paused = gameState;
    }

    public void LevelUp()
    {
        level++;
        damageBuff = (float)level * levelBonus_attackDmg;
        speedBuff = (float)level * levelBonus_attackSpeed;
        switch (attackSpeedType)
        {
            case AttackSpeedType.Fast:
                attackTime = (float)(1.0f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Medium:
                attackTime = (float)(1.5f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Slow:
                attackTime = (float)(2.0f / (fireSpeed + speedBuff));
                break;
        }

    }
    public void SetLevel(int lev)
    {
        level = lev;
        damageBuff = (float)level * levelBonus_attackDmg;
        speedBuff = (float)level * levelBonus_attackSpeed;
        switch (attackSpeedType)
        {
            case AttackSpeedType.Fast:
                attackTime = (float)(1.0f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Medium:
                attackTime = (float)(1.5f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Slow:
                attackTime = (float)(2.0f / (fireSpeed + speedBuff));
                break;
        }
    }

    public void AddTower()
    {
        added = true;
        if (damageType == DamageType.Special)
        {
            this.gameObject.GetComponent<SphereCollider>().radius = range * 1.5f;
        }
        else
        {
            this.gameObject.GetComponent<SphereCollider>().radius = range;
        }

        switch (attackSpeedType)
        {
            case AttackSpeedType.Fast:
                attackTime = (float)(1.0f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Medium:
                attackTime = (float)(1.5f / (fireSpeed + speedBuff));
                break;
            case AttackSpeedType.Slow:
                attackTime = (float)(2.0f / (fireSpeed + speedBuff));
                break;
        }
    }

    void Attack()
    {
        Vector2 angle = new Vector2(currentTarget.gameObject.transform.position.x - this.gameObject.transform.position.x, currentTarget.gameObject.transform.position.z - this.gameObject.transform.position.z);
        angle.Normalize();

        float rot = Mathf.Atan2(angle.y, angle.x) * 57.29578f;
        this.gameObject.transform.rotation = Quaternion.Euler(0, -rot + 180.0f, 0);
        if (towerType== TowerType.Archer)
        {
            SpawnProjectile();
            //currentTarget.GetComponent<EnemyController>().TakeDamage(damage, armorPen);

        }
        else if (towerType== TowerType.Fighter)
        {
            switch (damageType)
            {
                case DamageType.Single:
                    currentTarget.GetComponent<EnemyController>().TakeDamage(damage+damageBuff, armorPen);
                    

                    break;
                case DamageType.Spread:
                    foreach (GameObject enemy in enemiesToTarget)
                    {
                        if (enemy != null)
                        {
                            enemy.GetComponent<EnemyController>().TakeDamage((damage + damageBuff) * 0.1f, armorPen);
                        }
                        
                    }

                    break;
                case DamageType.Special:
                    currentTarget.GetComponent<EnemyController>().Slow(2.0f, 0.5f);
                    currentTarget.GetComponent<EnemyController>().TakeDamage(damage + damageBuff, armorPen);
                    break;
            }
        }
        else if (towerType == TowerType.Cannon)
        {
            switch (damageType)
            {
                case DamageType.Single:
                    SpawnProjectile();

                    break;
                case DamageType.Spread:
                    SpawnProjectile();

                    break;
                case DamageType.Special:
                    SpawnExplosion();
                    break;
            }
        }

    }

    void SpawnEffect()
    {

    }

    void SpawnExplosion()
    {
        // location, area
        //currentTarget.gameObject.transform.position
        GameObject explosive = Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
        explosive.transform.position = currentTarget.gameObject.transform.position;
        explosive.GetComponent<ExplosionController>().FireCannon(damage + damageBuff, aoeArea);
    }

    void SpawnProjectile()
    {
        //float angle = Mathf.Atan2((mouseY - player.gameObject.transform.position.y), (mouseX - player.gameObject.transform.position.x)) * 57.29578f;

        Vector2 angle = new Vector2(currentTarget.gameObject.transform.position.x - this.gameObject.transform.position.x, currentTarget.gameObject.transform.position.z - this.gameObject.transform.position.z);
        angle.Normalize();

        float rot = Mathf.Atan2(angle.y , angle.x) * 57.29578f;
        this.gameObject.transform.rotation = Quaternion.Euler(0, -rot + 180.0f, 0);
        GameObject currentProjectile;

        if (damageType == DamageType.Single)
        {
            currentProjectile = Instantiate(projectile, this.gameObject.transform.position, Quaternion.identity);
            currentProjectile.transform.rotation = Quaternion.Euler(0, -rot, 0);
            currentProjectile.GetComponent<ProjectileController>().FireProjectile(new Vector3(projectileSpeed * angle.x, 0, projectileSpeed * angle.y), damage + damageBuff, armorPen, passThrough + 1, projectileLife);
        }
        else if (damageType == DamageType.Spread)
        {
            float newRot;
            float sin;
            float cos;
            for (int i = 0; i < numOfShots; i++)
            {
                newRot = ((rot - (areaOfSpread/2.0f)) + ((float)i * (areaOfSpread/(numOfShots - 1.0f))));
                cos = Mathf.Cos(((rot - (areaOfSpread / 2.0f)) + (i * (areaOfSpread / (numOfShots - 1.0f)))) * Mathf.Deg2Rad);
                sin = Mathf.Sin(((rot - (areaOfSpread / 2.0f)) + (i * (areaOfSpread / (numOfShots - 1.0f)))) * Mathf.Deg2Rad);

                currentProjectile = Instantiate(projectile, this.gameObject.transform.position, Quaternion.identity);
                currentProjectile.transform.rotation = Quaternion.Euler(0, -newRot, 0);
                if (towerType == TowerType.Cannon)
                {
                    currentProjectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                currentProjectile.GetComponent<ProjectileController>().FireProjectile(new Vector3(projectileSpeed * 0.75f * cos, 0, projectileSpeed * 0.75f * sin), (damage + damageBuff) * 0.5f, armorPen, passThrough + 1, projectileLife);
            }
        }
        else
        {
            currentProjectile = Instantiate(projectile, this.gameObject.transform.position, Quaternion.identity);
            currentProjectile.transform.rotation = Quaternion.Euler(0, -rot, 0);
            currentProjectile.GetComponent<ProjectileController>().FireProjectile(new Vector3(projectileSpeed * 1.5f * angle.x, 0, projectileSpeed * 1.5f * angle.y), (damage + damageBuff) * 2.0f, armorPen, (passThrough + 1) * 3, projectileLife);
        }
    }

    void ChangeTarget(GameObject target)
    {
        if (target == null)
        {

        }
        else if (enemiesToTarget.Count == 0)
        {
            enemiesToTarget.Add(target);
            currentTarget = target;
            Debug.Log("first enemy");
        }
        else if (Vector3.Distance(target.transform.position, exit.transform.position) < Vector3.Distance(currentTarget.transform.position, exit.transform.position))
        {
            enemiesToTarget.Insert(0, target);
            currentTarget = target;
            Debug.Log("new enemy is closer");
        }
        else
        {
            enemiesToTarget.Add(target);
            Debug.Log("new enemy");
        }
        // on enemy enter range
        // - if (no enemys)
        //      add to target list, target
        // - else if (new enemy closer to exit)
        //      add target to beginning of list, target
        // - else 
        //      add to target list
        
    }

    public void AddTarget(GameObject target)
    {
        ChangeTarget(target);
    }

    public bool RemoveTarget(GameObject target)
    {
        if (enemiesToTarget.Contains(target))
        {
            if (enemiesToTarget.Count > 1)
            {
                enemiesToTarget.Remove(target);
                currentTarget = enemiesToTarget[0];
                Debug.Log("Multple enemies in range");
                return true;
            }
            else
            {
                enemiesToTarget.Remove(target);
                currentTarget = null;
                Debug.Log("last enemy in range");
                return true;
            }
        }
        return false;
    }

    public string GetAttackSpeed()
    {
        int whole = (int)(1.0f/ attackTime);
        int dec = (int)(((1.0f / attackTime) - whole) * 100);
        string text = whole.ToString() + "." + dec.ToString();

        return text;
    }

    public string GetAttackDmg()
    {
        int whole = (int)(damage + damageBuff);
        int dec = (int)(((damage + damageBuff) - whole) * 100);
        string text = whole.ToString() + "." + dec.ToString();

        return text;
    }

}
