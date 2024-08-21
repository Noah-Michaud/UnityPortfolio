using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    public bool playing = false;
    public PlayerController player;
    float count = 0.0f;
    bool countup = true;
    bool counting = false;
    public float arrowPower = 20.0f;
    float maxCount = 1.5f;

    public GameObject powerBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            player.UpdateBow(GetShotAngle());
            if (player.canFire && Input.GetMouseButton(0))
            {
                if (!counting)
                {
                    counting = true;
                    countup = true;
                    count = 0.0f;
                }
                else if (counting && countup)
                {
                    count += Time.deltaTime;
                    UpdatePowerBar();
                    if (count >= maxCount)
                    {
                        countup = false;
                    }
                }
                else if (counting && !countup)
                {
                    count -= Time.deltaTime;
                    UpdatePowerBar();
                    if (count <= 0.0f)
                    {
                        countup = true;
                    }
                }
            }
            else if (player.canFire && counting)
            {
                player.TryFire(GetShotVector(), +(count + 0.2f) * arrowPower);
                counting = false;
                count = 0.0f;
                UpdatePowerBar();
            }
        }
        
    }

    Vector2 GetShotVector()
    {
        float mouseX = (((Input.mousePosition.x / 1920f) * 21.4f) - 10.7f);
        float mouseY = ((Input.mousePosition.y / 1080f) * 12f);
        Vector2 angle = new Vector2(mouseX - player.gameObject.transform.position.x, mouseY - player.gameObject.transform.position.y);
        angle.Normalize();
        return angle;
    }

    float GetShotAngle()
    {
        float mouseX = (((Input.mousePosition.x / 1920f) * 21.4f) - 10.7f);
        float mouseY = ((Input.mousePosition.y / 1080f) * 12f);
        float angle = Mathf.Atan2((mouseY - player.gameObject.transform.position.y),(mouseX - player.gameObject.transform.position.x)) * 57.29578f;
        return angle;
    }

    void UpdatePowerBar()
    {
        
        // Changes size and color
        if (count <= 0.0f)
        {
            powerBar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 40f);
        }
        else if (count < maxCount*0.5f)
        {
            powerBar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(190.0f * (count/maxCount), 40f);
            powerBar.GetComponent<Image>().color = new Color(1, ((2.0f* count) / maxCount), 0, 1);
        }
        else if (count >= maxCount * 0.5f)
        {
            powerBar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(190.0f * (count / maxCount), 40f);
            powerBar.GetComponent<Image>().color = new Color(1.0f - ((count - (0.5f*maxCount))/maxCount), 1, 0, 1);
        }
        else if (count >= maxCount)
        {
            powerBar.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(190.0f, 40f);
            powerBar.GetComponent<Image>().color = new Color(0, 1, 0, 1);
        }

    }

}
