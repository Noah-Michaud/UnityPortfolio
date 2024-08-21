using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : MonoBehaviour
{
    float[] mousePos = new float[2];
    public GameObject shiftLever;
    public GameObject rubberBand;
    Rigidbody rb;
    public MouseController mouse;
    float xPos = 0.0f;
    float yPos = 0.0f;

    public GameObject shiftKnob;

    //bool shifting = false;
    // Start is called before the first frame update
    void Start()
    {
        //rb = rubberBand.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        shiftKnob.transform.localPosition = new Vector3(shiftLever.transform.position.x * 100.0f, shiftLever.transform.position.z * 100.0f, 0);
    }

    public void Shift(float[] mouseData)
    {
        mousePos = mouseData;
        xPos = rubberBand.transform.position.x;
        yPos = rubberBand.transform.position.z;
        //Vector2 angle = new Vector2((mousePos[0] / 100.0f) - xPos, (mousePos[1] / 100.0f) - yPos);
        //angle.Normalize();
        rubberBand.transform.position = new Vector3((mousePos[0] / 100.0f), 0.0f, (mousePos[1] / 100.0f));
        //rb.AddForce(new Vector3(angle.x, 0, angle.y) * 1.0f, ForceMode.Force);
    }
}
