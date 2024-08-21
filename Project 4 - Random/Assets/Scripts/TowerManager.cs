using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// this controls the ability to buy, sell, reroll towers
/// </summary>
public class TowerManager : MonoBehaviour
{
    public GameObject tower_archer_shortbow;
    public GameObject tower_archer_spread;
    public GameObject tower_archer_longbow;

    public GameObject tower_cannon_single;
    public GameObject tower_cannon_spread;
    public GameObject tower_cannon_explosive;

    public GameObject tower_fighter_spear;
    public GameObject tower_fighter_sword;
    public GameObject tower_fighter_brawler;

    // text stuff
    string archer_shortbow_info = "(1) Shortbow: Quickly fires single arrows";
    string archer_spread_info = "(2) Spreadshot: Fires three low damage arrows in a cone";
    string archer_longbow_info = "(3) Longbow(AP)(PT): Slowly fires a powerful arrow";
                              
    string cannon_single_info = "(1) Solidshot(AP)(PT): Fires a single shot that penetrates enemies";
    string cannon_spread_info = "(2) Grapeshot(AP)(PT): Fires a cluster of small, low damage shots";
    string cannon_explosive_info = "(3) Mortar(AP): Launches a bomb that explodes an area";
                            
    string fighter_spear_info = "(1) Fencer(AP): Quicky stabs enemies as they pass";
    string fighter_sword_info = "(2) Slasher: Swings sword aound, damaging all nearby enemies";
    string fighter_brawler_info = "(3) Brawler: Attacks nearby enemies, slowing them";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetTowerInfoText(TowerController.TowerType towerType)
    {
        string[] infoText = new string[3];
        switch (towerType)
        {
            case TowerController.TowerType.Archer:
                infoText[0] = archer_shortbow_info;
                infoText[1] = archer_spread_info;
                infoText[2] = archer_longbow_info;
                break;
            case TowerController.TowerType.Cannon:
                infoText[0] = cannon_single_info;
                infoText[1] = cannon_spread_info;
                infoText[2] = cannon_explosive_info;
                break;
            case TowerController.TowerType.Fighter:
                infoText[0] = fighter_spear_info;
                infoText[1] = fighter_sword_info;
                infoText[2] = fighter_brawler_info;
                break;
        }
        return infoText;
    }

    public GameObject PlaceTower(TowerController.TowerType towerType, Vector3 pos)
    {
        Debug.Log("Trying to place tower");
        GameObject prefab = RollTower(towerType);
        if (prefab != null)
        {
            GameObject newTower = Instantiate(prefab, pos, Quaternion.identity);
            newTower.GetComponent<TowerController>().AddTower();
            return newTower;
        }
        else
        {
            Debug.Log("prefab is null");
            return null;
        }
        
        
        // rand roll and instantiate
        
    }

    public string GetSubtype(TowerController tower)
    {
        string sub = "";
        TowerController.TowerType towerType = tower.towerType;
        switch (towerType)
        {
            case TowerController.TowerType.Archer:
                if (tower.damageType == TowerController.DamageType.Single)
                {
                    sub = "Shortbow";
                }
                else if (tower.damageType == TowerController.DamageType.Spread)
                {
                    sub = "Spreadshot";
                }
                else
                {
                    sub = "Longbow";
                }
                break;
            case TowerController.TowerType.Cannon:
                if (tower.damageType == TowerController.DamageType.Single)
                {
                    sub = "SolidShot";
                }
                else if (tower.damageType == TowerController.DamageType.Spread)
                {
                    sub = "Grapeshot";
                }
                else
                {
                    sub = "Mortar";
                }
                break;
            case TowerController.TowerType.Fighter:
                if (tower.damageType == TowerController.DamageType.Single)
                {
                    sub = "Fencer";
                }
                else if (tower.damageType == TowerController.DamageType.Spread)
                {
                    sub = "Slasher";
                }
                else
                {
                    sub = "Brawler";
                }
                break;
        }
        return sub;
    }

    public GameObject RerollTower(GameObject tower)
    {
        GameObject newTowerType = RollTower(tower.GetComponent<TowerController>().towerType);
        GameObject newTower;
        if (newTowerType.GetComponent<TowerController>().damageType == tower.GetComponent<TowerController>().damageType)
        {
            tower.GetComponent<TowerController>().LevelUp();
            return tower;
        }
        else
        {
            Vector3 oldPos = new Vector3(tower.transform.position.x, tower.transform.position.y, tower.transform.position.z);
            newTower = Instantiate(newTowerType, oldPos, Quaternion.identity);
            newTower.GetComponent<TowerController>().SetLevel(tower.GetComponent<TowerController>().level);
            newTower.GetComponent<TowerController>().AddTower();
            Destroy(tower);
            return newTower;
        }


        return tower;
        // if towertype = new towertype, level up
        // if towertype != new towertype, instantiate new tower, level up to same level, destroy old tower
    }

    GameObject RollTower(TowerController.TowerType towerType)
    {
        GameObject towerRoll = null;
        float rollf = Random.Range(0.0f, 2.9999f);
        Debug.Log("rolled " + rollf);
        int roll = (int)(rollf);
        Debug.Log("rolled " + roll.ToString());
        switch (towerType)
        {
            case TowerController.TowerType.Archer:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_archer_shortbow;
                        break;
                    case 1:
                        towerRoll = tower_archer_spread;
                        break;
                    case 2:
                        towerRoll = tower_archer_longbow;
                        break;
                }
                break;
            case TowerController.TowerType.Cannon:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_cannon_single;
                        break;
                    case 1:
                        towerRoll = tower_cannon_spread;
                        break;
                    case 2:
                        towerRoll = tower_cannon_explosive;
                        break;
                }
                break;
            case TowerController.TowerType.Fighter:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_fighter_spear;
                        break;
                    case 1:
                        towerRoll = tower_fighter_sword;
                        break;
                    case 2:
                        towerRoll = tower_fighter_brawler;
                        break;
                }
                break;
        }
        Debug.Log("Rolled Tower");
        return towerRoll;
    }

    GameObject RollTower(TowerController.TowerType towerType, int roll)
    {
        GameObject towerRoll = null;
        switch (towerType)
        {
            case TowerController.TowerType.Archer:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_archer_shortbow;
                        break;
                    case 1:
                        towerRoll = tower_archer_spread;
                        break;
                    case 2:
                        towerRoll = tower_archer_longbow;
                        break;
                }
                break;
            case TowerController.TowerType.Cannon:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_cannon_single;
                        break;
                    case 1:
                        towerRoll = tower_cannon_spread;
                        break;
                    case 2:
                        towerRoll = tower_cannon_explosive;
                        break;
                }
                break;
            case TowerController.TowerType.Fighter:
                switch (roll)
                {
                    case 0:
                        towerRoll = tower_fighter_spear;
                        break;
                    case 1:
                        towerRoll = tower_fighter_sword;
                        break;
                    case 2:
                        towerRoll = tower_fighter_brawler;
                        break;
                }
                break;
        }
        return towerRoll;
    }
}
