using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 2;
    float startMovementSpeed;

    bool isGrounded = true;
    bool isCrouched = false;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    public float viewRange;

    public CapsuleCollider sc;

    int hasCollision;

    Rigidbody rb;

    public static GameObject playerObject;

    //Settings
    private KeyCode sprint;
    public KeyCode Sprint
    {
        get
        {
            return sprint;
        }
    }
    private KeyCode crouch;
    public KeyCode Crouch
    {
        get
        {
            return crouch;
        }
    }
    private KeyCode mforw;
    public KeyCode Mforw
    {
        get
        {
            return mforw;
        }
    }
    private KeyCode mback;
    public KeyCode Mback
    {
        get
        {
            return mback;
        }
    }
    private KeyCode mright;
    public KeyCode Mright
    {
        get
        {
            return mright;
        }
    }
    private KeyCode mleft;
    public KeyCode Mleft
    {
        get
        {
            return mleft;
        }
    }
    private KeyCode jump;
    public KeyCode Jump
    {
        get
        {
            return jump;
        }
    }

    private void ListenToSettings()
    {
        //Sprint
        Setting spr = SettingsManager.GetInstance().Settings[0].GetSetting("sprint");
        spr.changeEvent += (sender, value) =>
        {
            sprint = (KeyCode)value;
        };
        sprint = (KeyCode)spr.Value;

        //Crouch
        Setting crch = SettingsManager.GetInstance().Settings[0].GetSetting("crouch");
        crch.changeEvent += (sender, value) =>
        {
            crouch = (KeyCode)value;
        };
        crouch = (KeyCode)crch.Value;

        //Forward
        Setting forw = SettingsManager.GetInstance().Settings[0].GetSetting("mforw");
        forw.changeEvent += (sender, value) =>
        {
            mforw = (KeyCode)value;
        };
        mforw = (KeyCode)forw.Value;

        //Forward
        Setting back = SettingsManager.GetInstance().Settings[0].GetSetting("mback");
        back.changeEvent += (sender, value) =>
        {
            mback = (KeyCode)value;
        };
        mback = (KeyCode)back.Value;

        //Right
        Setting right = SettingsManager.GetInstance().Settings[0].GetSetting("mright");
        right.changeEvent += (sender, value) =>
        {
            mright = (KeyCode)value;
        };
        mright = (KeyCode)right.Value;

        //Left
        Setting left = SettingsManager.GetInstance().Settings[0].GetSetting("mleft");
        left.changeEvent += (sender, value) =>
        {
            mleft = (KeyCode)value;
        };
        mleft = (KeyCode)left.Value;

        //Jump
        Setting jmp = SettingsManager.GetInstance().Settings[0].GetSetting("jump");
        jmp.changeEvent += (sender, value) =>
        {
            jump = (KeyCode)value;
        };
        jump = (KeyCode)jmp.Value;
    }

    // Start is called before the first frame update
    void Start()
    {
        ListenToSettings();
        ///initializes the settings and components
        sc.height = 1.7f;

        startMovementSpeed = movementSpeed;
        hasCollision = 0;

        rb = GetComponent<Rigidbody>();

        playerObject = GameObject.Find("Player");
    }
    
    void Update()
    {
        
        if (Cursor.lockState == CursorLockMode.Locked && !TutorialManager.cutsceneLock)
        {
            //uses the mouse to rotate the player GameObject
            transform.eulerAngles = new Vector3(Mathf.Clamp(CameraMovement.pitch, -viewRange, viewRange), CameraMovement.yaw, 0);

            //increases speed when Left Shift is pressed
            if (Input.GetKey(Sprint) && isCrouched == false)
            {
                movementSpeed = startMovementSpeed * 2;
            }
            else if (Input.GetKey(Sprint) && isCrouched == true)
            {
                movementSpeed = startMovementSpeed / 2;
            }
            else if (isCrouched == true)
            {
                movementSpeed = startMovementSpeed / 2;
            }
            else
            {
                movementSpeed = startMovementSpeed;
            }

            //walk when the appropiate key is pressed
            if (Input.GetKey(Mforw))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(Mback))
            {
                transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(Mright))
            {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(Mleft))
            {
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            }

            //adds crouching function which makes the collider smaller when Left control is pressed and makes it larger when released
            if (Input.GetKey(Crouch))
            {
                sc.height = 0.9f;
                isCrouched = true;
            }
            if (Input.GetKeyUp(Crouch))
            {
                sc.height = 1.7f;
                isCrouched = false;
            }



            //Jump when space is pressed
            if (Input.GetKeyDown(Jump) && isGrounded == true)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 4, 0), ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    void OnCollisionEnter(Collision theCollision)
    {
        isGrounded = true;
    }

    void OnCollision(Collision theCollision)
    {
        //if no key is pressed, the rigidbody becomes kinematic (no movement at all) but only while touching something
        if (Input.anyKey)
        {
            rb.isKinematic = false;
        }else{
             rb.isKinematic = true;
        }
    }

    public static void TeleportPlayer(float x, float y, float z)
    {
        playerObject.GetComponent<Transform>().position = new Vector3(x, y, z);
        playerObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
