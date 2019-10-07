using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    float MovementSpeed = 1;

    bool isGrounded = true;
    bool isCrouched = false;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public CapsuleCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        sc.height = 1;
         Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        //code voor het rennen wanneer linkershift wordt ingedrukt
        if(Input.GetKey(KeyCode.LeftShift) && isCrouched == false){
            MovementSpeed = 3;
        }else if(Input.GetKey(KeyCode.LeftShift) && isCrouched == true) {
            MovementSpeed = 0.5f;
        }else if(isCrouched == true){
            MovementSpeed = 0.5f;
        }else{
            MovementSpeed = 1;
        }

        //code voor het lopen op basis van de ingedrukte letter
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed);
        }

        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * Time.deltaTime * MovementSpeed);
        }

        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * Time.deltaTime * MovementSpeed);
        }

        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * Time.deltaTime * MovementSpeed);
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
