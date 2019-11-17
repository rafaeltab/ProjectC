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

    // Start is called before the first frame update
    void Start()
    {
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
            if (Input.GetKey(KeyCode.LeftShift) && isCrouched == false)
            {
                movementSpeed = startMovementSpeed * 2;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && isCrouched == true)
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
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            }

            //adds crouching function which makes the collider smaller when Left control is pressed and makes it larger when released
            if (Input.GetKey(KeyCode.LeftControl))
            {
                sc.height = 0.9f;
                isCrouched = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                sc.height = 1.7f;
                isCrouched = false;
            }



            //Jump when space is pressed
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
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
        //if no key is pressed, the rigidbody becomes kinematic (no movement at all)
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
    }
}
