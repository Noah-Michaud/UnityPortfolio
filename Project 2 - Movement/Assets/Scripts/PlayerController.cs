using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
//using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    public MouseController mouseController;
    public CameraController cam;
    public HandHoldSpwner holdSpawner;
    public GameController game;

    //public GameObject body; //
    public GameObject chestCenter; //
    //public GameObject torso; // 
    public GameObject sholders; // 
    public GameObject hips; // 
    public GameObject head; // 

    public GameObject rightArm;
    public GameObject rightElbow;
    public GameObject rightFoot;
    public GameObject rightShin;
    public GameObject rightThigh;

    public GameObject leftArm;
    public GameObject leftElbow;
    public GameObject leftFoot;
    public GameObject leftShin;
    public GameObject leftThigh;



    public GameObject rightSholder;
    public GameObject rightForeArm;
    public GameObject rightHand;

    public GameObject leftSholder;
    public GameObject leftForeArm;
    public GameObject leftHand;

    public GameObject rightHip;
    public GameObject rightknee;

    public GameObject leftHip;
    public GameObject leftknee;

    //Vector3 bodyPos;
    Vector3 chestCenterPos;
    //Vector3 torsoPos;
    Vector3 sholdersPos;
    Vector3 hipsPos;
    Vector3 headPos;
    Vector3 rightArmPos;
    Vector3 rightElbowPos;
    Vector3 rightFootPos;
    Vector3 rightShinPos;
    Vector3 rightThighPos;
    Vector3 leftArmPos;
    Vector3 leftElbowPos;
    Vector3 leftFootPos;
    Vector3 leftShinPos;
    Vector3 leftThighPos;
    Vector3 rightSholderPos;
    Vector3 rightForeArmPos;
    Vector3 rightHandPos;
    Vector3 leftSholderPos;
    Vector3 leftForeArmPos;
    Vector3 leftHandPos;
    Vector3 rightHipPos;
    Vector3 rightkneePos;
    Vector3 leftHipPos;
    Vector3 leftkneePos;

    //Quaternion bodyRot;
    Quaternion chestCenterRot;
    //Quaternion torsoRot;
    Quaternion sholdersRot;
    Quaternion hipsRot;
    Quaternion headRot;
    Quaternion rightArmRot;
    Quaternion rightElbowRot;
    Quaternion rightFootRot;
    Quaternion rightShinRot;
    Quaternion rightThighRot;
    Quaternion leftArmRot;
    Quaternion leftElbowRot;
    Quaternion leftFootRot;
    Quaternion leftShinRot;
    Quaternion leftThighRot;
    Quaternion rightSholderRot;
    Quaternion rightForeArmRot;
    Quaternion rightHandRot;
    Quaternion leftSholderRot;
    Quaternion leftForeArmRot;
    Quaternion leftHandRot;
    Quaternion rightHipRot;
    Quaternion rightkneeRot;
    Quaternion leftHipRot;
    Quaternion leftkneeRot;

    HingeJoint rightSholderHinge;
    HingeJoint rightForeArmHinge;

    HingeJoint leftSholderHinge;
    HingeJoint leftForeArmHinge;

    HingeJoint rightHipHinge;
    HingeJoint rightkneeHinge;

    HingeJoint leftHipHinge;
    HingeJoint leftkneeHinge;


    JointSpring rightSholderSpring;
    JointSpring rightForeArmSpring;
                            
    JointSpring leftSholderSpring;
    JointSpring leftForeArmSpring;
                            
    JointSpring rightHipSpring;
    JointSpring rightkneeSpring;
                            
    JointSpring leftHipSpring;
    JointSpring leftkneeSpring;


    bool rightHandLocked = true;
    bool leftHandLocked = true;

    bool leftHandCanGrab = false;
    bool rightHandCanGrab = false;

    int vertDirect = 0;
    int horizDirect = 0;

    bool[] inputBool; 
    bool[] lastFrame;

    float highestPoint = 0.0f;

    bool leftWasLock = false;
    bool rightWasLock = false;

    bool playing = false;
    bool frozen = false;
    bool freeFall = false;
    // Start is called before the first frame update
    void Start()
    {
        SetStartPosRot();
        inputBool = new bool[3] {false, false, false };
        lastFrame = new bool[3] { false, false, false };


        SetJoints();

    }                     
                          
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Escape))
        {
            game.TryPause();
        }
        if (playing == false && Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            playing = true;
            if (frozen == true)
            {
                PausePlayer(false);
            }
            
        }
        
        if (playing == true)
        {
            if (leftHand.transform.position.y + 20.0f < cam.gameObject.transform.position.y)
            {
                playing = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                inputBool[0] = true;
            }
            else
            {
                inputBool[0] = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputBool[1] = true;
            }
            else
            {
                inputBool[1] = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputBool[2] = true;
            }
            else
            {
                inputBool[2] = false;
            }
            ArmControl(mouseController.GetMouseData());
            MovementInput();
        }
        
        
    }

    void MovementInput()
    {
        if (inputBool[0])
        {
            // start motor
            ArmSprings(true);
        }
        else if (!inputBool[0])
        {
            // turn off arm motor
            ArmSprings(false);
        }

        if (inputBool[1] && inputBool[2])
        {
            //LegSprings(false, 0.0f);
        }
        else if (inputBool[1])
        {
            // start leg motor left
            //LegSprings(true, 1.0f);

        }
        else if (inputBool[2])
        {
            // start motor right
            //LegSprings(true, -1.0f);
        }
        else
        {
            // turn off motor
            //LegSprings(false, 0.0f);
        }



        lastFrame[0] = inputBool[0];
        lastFrame[1] = inputBool[1];
        lastFrame[2] = inputBool[2];
    }


    void ArmControl(float[] data)
    {
        // check if colliding with handholds


        // 0 = x, 1 = y, 2 = left, 3 = right
        if (data[2] > 0.0f && leftHandLocked == false && leftHandCanGrab)
        {
            // left mouse down
            LockHand(true, 0);
        }
        else if (data[2] < 0.0f && leftHandLocked == true)
        {
            LockHand(false, 0);
        }
        if (data[3] > 0.0f && rightHandLocked == false && rightHandCanGrab)
        {
            // right mouse down
            LockHand(true, 1);
        }
        else if (data[3] < 0.0f && rightHandLocked == true)
        {
            LockHand(false, 1);
        }


        //MoveHands();
    }

    void MoveHands()
    {
        if (leftHandLocked == false)
        {

        }
        if (rightHandLocked == false)
        {

        }
    }

    void LockHand(bool turnedOn, int hand)
    {
        if (hand == 0)
        {
            // lock/unlock left hand
            if (turnedOn)
            {
                mouseController.EnableSprings(hand, 0);
                leftHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                leftHandLocked= true;
                UpdateCamera();
                UpdateMaxHeight();
                game.UpdateHeight(leftHand.gameObject.transform.position.y, rightHand.gameObject.transform.position.y);

            }
            else
            {
                mouseController.EnableSprings(hand, 1);
                leftHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                leftHandLocked= false;
                UpdateCamera();
            }

        }
        else if (hand == 1)
        {
            // lock/unlock right hand
            if (turnedOn)
            {
                mouseController.EnableSprings(hand, 0);
                rightHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                rightHandLocked= true;
                UpdateCamera();
                UpdateMaxHeight();
                game.UpdateHeight(leftHand.gameObject.transform.position.y, rightHand.gameObject.transform.position.y);
            }
            else
            {
                mouseController.EnableSprings(hand, 1);
                rightHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                rightHandLocked= false;
                UpdateCamera();
            }
        }
        if (freeFall == true)
        {
            if (leftHandLocked)
            {
                freeFall = false;
                mouseController.EnableSprings(1, 1);
            }
            else if (rightHandLocked)
            {
                freeFall = false;
                mouseController.EnableSprings(0, 1);
            }
        }

        if (!leftHandLocked && !rightHandLocked)
        {
            mouseController.EnableSprings(0, 0);
            mouseController.EnableSprings(1, 0);
            freeFall = true;
        }
        
        
    }

    void ArmSprings(bool turnedOn)
    {
        if (rightHandLocked)
        {
            rightSholder.GetComponent<HingeJoint>().useSpring = turnedOn;
            rightForeArm.GetComponent<HingeJoint>().useSpring = turnedOn;
        }
        if (leftHandLocked)
        {
            leftSholder.GetComponent<HingeJoint>().useSpring = turnedOn;
            leftForeArm.GetComponent<HingeJoint>().useSpring = turnedOn;
        }
    }

    

    void LegSprings(bool turnedOn, float direction)
    {
        rightHip.GetComponent<HingeJoint>().useSpring = turnedOn;
        rightknee.GetComponent<HingeJoint>().useSpring = turnedOn;
        leftHip.GetComponent<HingeJoint>().useSpring = turnedOn;
        leftknee.GetComponent<HingeJoint>().useSpring = turnedOn;

        rightHipSpring.targetPosition = 130.0f * direction;
        rightkneeSpring.targetPosition = 130.0f * direction;
        leftHipSpring.targetPosition = 130.0f * direction;
        leftkneeSpring.targetPosition = 130.0f * direction;

        SetJoints();
    }

    void SetJoints()
    {



        rightSholderHinge = rightSholder.GetComponent<HingeJoint>();
        rightForeArmHinge = rightForeArm.GetComponent<HingeJoint>();

        leftSholderHinge  = leftSholder.GetComponent<HingeJoint>();
        leftForeArmHinge  = leftForeArm.GetComponent<HingeJoint>();

        rightHipHinge    = rightHip.GetComponent<HingeJoint>();
        rightkneeHinge    = rightknee.GetComponent<HingeJoint>();

        leftHipHinge     = leftHip.GetComponent<HingeJoint>();
        leftkneeHinge     = leftknee.GetComponent<HingeJoint>();




        rightSholderSpring = rightSholderHinge.spring;
        rightForeArmSpring = rightForeArmHinge.spring;
                                            
        leftSholderSpring = leftSholderHinge.spring; 
        leftForeArmSpring = leftForeArmHinge.spring; 
                                            
        rightHipSpring = rightHipHinge.spring;    
        rightkneeSpring = rightkneeHinge.spring;   
                                           
        leftHipSpring = leftHipHinge.spring;     
        leftkneeSpring = leftkneeHinge.spring;    
    }

    public void HandCanGrab(int hand, bool can)
    {
        if (hand == 0)
        {
            leftHandCanGrab = can;
        }
        else if (hand == 1)
        {
            rightHandCanGrab = can;
        }
    }

    void UpdateCamera()
    {
        float lBool = 0.0f;
        float rBool = 0.0f;
        if (leftHandLocked)
        {
            lBool = 1.0f;
        }
        if (rightHandLocked)
        {
            rBool = 1.0f;
        }

        cam.UpdateCamera(lBool, rBool, leftHand.gameObject.transform.position.y, rightHand.gameObject.transform.position.y);

    }

    void UpdateMaxHeight()
    {
        if (leftHandLocked)
        {
            if (leftHand.transform.position.y > highestPoint)
            {
                highestPoint = leftHand.transform.position.y;
            }
        }
        if (rightHandLocked)
        {
            if (rightHand.transform.position.y > highestPoint)
            {
                highestPoint = rightHand.transform.position.y;
            }
        }
        holdSpawner.UpdatePlayerPos(highestPoint);
    }

    public void PausePlayer(bool pause)
    {
        if (pause)
        {
            if (leftHandLocked)
            {
                leftWasLock = true;
            }
            if (rightHandLocked)
            {
                rightWasLock = true;
            }
            playing = false;
            frozen = true;
            //body           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            chestCenter    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //torso          .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            sholders       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            hips           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            head           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightArm       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightElbow     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightFoot      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightShin      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightThigh     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftArm        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftElbow      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftFoot       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftShin       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftThigh      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightSholder   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightForeArm   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightHand      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftSholder    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftForeArm    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftHand       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightHip       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            rightknee      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftHip        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            leftknee       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (!pause)
        {
            //body           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            chestCenter    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //torso          .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            sholders       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            hips           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            head           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightArm       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightElbow     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightFoot      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightShin      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightThigh     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftArm        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftElbow      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftFoot       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftShin       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftThigh      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightSholder   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightForeArm   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightHand      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftSholder    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftForeArm    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftHand       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightHip       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rightknee      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftHip        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            leftknee       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //body           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            chestCenter    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            //torso          .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            sholders       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            hips           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            head           .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightArm       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightElbow     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightFoot      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightShin      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightThigh     .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftArm        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftElbow      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftFoot       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftShin       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftThigh      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightSholder   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightForeArm   .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightHand      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftSholder    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftForeArm    .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftHand       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightHip       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            rightknee      .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftHip        .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            leftknee       .GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

            frozen = false;
            if (leftWasLock)
            {
                LockHand(true, 0);
                leftWasLock = false;
            }
            if (rightWasLock)
            {
                LockHand(true, 1);
                rightWasLock = false;
            }
        }
    }

    public void ResetPlayer()
    {



        if(true)
        {
                         //body.transform.position= bodyPos            ;
              chestCenter    .transform.position= chestCenterPos     ;
              //torso          .transform.position= torsoPos           ;
              sholders       .transform.position= sholdersPos        ;
              hips           .transform.position= hipsPos            ;
              head           .transform.position= headPos            ;
              rightArm       .transform.position= rightArmPos        ;
              rightElbow     .transform.position= rightElbowPos      ;
              rightFoot      .transform.position= rightFootPos       ;
              rightShin      .transform.position= rightShinPos       ;
              rightThigh     .transform.position= rightThighPos      ;
              leftArm        .transform.position= leftArmPos         ;
              leftElbow      .transform.position= leftElbowPos       ;
              leftFoot       .transform.position= leftFootPos        ;
              leftShin       .transform.position= leftShinPos        ;
              leftThigh      .transform.position= leftThighPos       ;
              rightSholder   .transform.position= rightSholderPos    ;
              rightForeArm   .transform.position= rightForeArmPos    ;
              rightHand      .transform.position= rightHandPos       ;
              leftSholder    .transform.position= leftSholderPos     ;
              leftForeArm    .transform.position= leftForeArmPos     ;
              leftHand       .transform.position= leftHandPos        ;
              rightHip       .transform.position= rightHipPos        ;
              rightknee      .transform.position= rightkneePos       ;
              leftHip        .transform.position= leftHipPos         ;
            leftknee.transform.position = leftkneePos;
            
            // body           .transform.rotation= bodyRot             ;
             chestCenter    .transform.rotation= chestCenterRot      ;
             //torso          .transform.rotation= torsoRot            ;
             sholders       .transform.rotation= sholdersRot         ;
             hips           .transform.rotation= hipsRot             ;
             head           .transform.rotation= headRot             ;
             rightArm       .transform.rotation= rightArmRot         ;
             rightElbow     .transform.rotation= rightElbowRot       ;
             rightFoot      .transform.rotation= rightFootRot        ;
             rightShin      .transform.rotation= rightShinRot        ;
             rightThigh     .transform.rotation= rightThighRot       ;
             leftArm        .transform.rotation= leftArmRot          ;
             leftElbow      .transform.rotation= leftElbowRot        ;
             leftFoot       .transform.rotation= leftFootRot         ;
             leftShin       .transform.rotation= leftShinRot         ;
             leftThigh      .transform.rotation= leftThighRot        ;
             rightSholder   .transform.rotation= rightSholderRot     ;
             rightForeArm   .transform.rotation= rightForeArmRot     ;
             rightHand      .transform.rotation= rightHandRot        ;
             leftSholder    .transform.rotation= leftSholderRot      ;
             leftForeArm    .transform.rotation= leftForeArmRot      ;
             leftHand       .transform.rotation= leftHandRot         ;
             rightHip       .transform.rotation= rightHipRot         ;
             rightknee      .transform.rotation= rightkneeRot        ;
             leftHip        .transform.rotation= leftHipRot          ;
             leftknee       .transform.rotation = leftkneeRot;

            playing = false;
            cam.ResetCamera(leftHand.gameObject.transform.position.y, rightHand.gameObject.transform.position.y);
            highestPoint = leftHand.gameObject.transform.position.y;
            holdSpawner.UpdatePlayerPos(leftHand.gameObject.transform.position.y);
        }
    }

    void SetStartPosRot()
    {
        //bodyPos               = body.transform.position;
        chestCenterPos        =   chestCenter    .transform.position;
        //torsoPos              =   torso          .transform.position;
        sholdersPos           =   sholders       .transform.position;
        hipsPos               =   hips           .transform.position;
        headPos               =   head           .transform.position;
        rightArmPos           =   rightArm       .transform.position;
        rightElbowPos         =   rightElbow     .transform.position;
        rightFootPos          =   rightFoot      .transform.position;
        rightShinPos          =   rightShin      .transform.position;
        rightThighPos         =   rightThigh     .transform.position;
        leftArmPos            =   leftArm        .transform.position;
        leftElbowPos          =   leftElbow      .transform.position;
        leftFootPos           =   leftFoot       .transform.position;
        leftShinPos           =   leftShin       .transform.position;
        leftThighPos          =   leftThigh      .transform.position;
        rightSholderPos       =   rightSholder   .transform.position;
        rightForeArmPos       =   rightForeArm   .transform.position;
        rightHandPos          =   rightHand      .transform.position;
        leftSholderPos        =   leftSholder    .transform.position;
        leftForeArmPos        =   leftForeArm    .transform.position;
        leftHandPos           =   leftHand       .transform.position;
        rightHipPos           =   rightHip       .transform.position;
        rightkneePos          =   rightknee      .transform.position;
        leftHipPos            =   leftHip        .transform.position;
        leftkneePos           =   leftknee.transform.position;

        //bodyRot          =     body        .transform.rotation;
        chestCenterRot   =  chestCenter    .transform.rotation;
        //torsoRot         =  torso          .transform.rotation;
        sholdersRot      =  sholders       .transform.rotation;
        hipsRot          =  hips           .transform.rotation;
        headRot          =  head           .transform.rotation;
        rightArmRot      =  rightArm       .transform.rotation;
        rightElbowRot    =  rightElbow     .transform.rotation;
        rightFootRot     =  rightFoot      .transform.rotation;
        rightShinRot     =  rightShin      .transform.rotation;
        rightThighRot    =  rightThigh     .transform.rotation;
        leftArmRot       =  leftArm        .transform.rotation;
        leftElbowRot     =  leftElbow      .transform.rotation;
        leftFootRot      =  leftFoot       .transform.rotation;
        leftShinRot      =  leftShin       .transform.rotation;
        leftThighRot     =  leftThigh      .transform.rotation;
        rightSholderRot  =  rightSholder   .transform.rotation;
        rightForeArmRot  =  rightForeArm   .transform.rotation;
        rightHandRot     =  rightHand      .transform.rotation;
        leftSholderRot   =  leftSholder    .transform.rotation;
        leftForeArmRot   =  leftForeArm    .transform.rotation;
        leftHandRot      =  leftHand       .transform.rotation;
        rightHipRot      =  rightHip       .transform.rotation;
        rightkneeRot     =  rightknee      .transform.rotation;
        leftHipRot       =  leftHip        .transform.rotation;
        leftkneeRot      =         leftknee.transform.rotation;

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
