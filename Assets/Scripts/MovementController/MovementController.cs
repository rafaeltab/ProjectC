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

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float viewRange;

    public CapsuleCollider sc;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        sc.height = 1;
        
        startMovementSpeed = movementSpeed;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3( Mathf.Clamp( pitch, -viewRange, viewRange), yaw, 0);

        //code voor het rennen wanneer linkershift wordt ingedrukt
        if(Input.GetKey(KeyCode.LeftShift) && isCrouched == false){
            movementSpeed = startMovementSpeed * 2;
        }else if(Input.GetKey(KeyCode.LeftShift) && isCrouched == true) {
            movementSpeed = startMovementSpeed / 2;
        }else if(isCrouched == true){
            movementSpeed = startMovementSpeed / 2;
        }else{
            movementSpeed = startMovementSpeed;
        }

        //code voor het lopen op basis van de ingedrukte letter
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.LeftControl)){
             sc.height = 0.5f;
             isCrouched = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)){
             sc.height = 1;
             isCrouched = false;
        }

        //code voor springen als spatie is ingedrukt
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true){
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        }
         
    }

    void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "floor")
        {
             isGrounded = true;
        }    
    }

    void OnCollisionExit(Collision theCollision)
    {
         if (theCollision.gameObject.tag == "floor")
        {
             isGrounded = false;
        }
    }
}
