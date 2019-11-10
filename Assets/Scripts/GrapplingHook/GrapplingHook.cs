using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hook;
    public GameObject hookHolder;

    public float hookTravelSpeed;
    public float playerTravelSpeed;

    public static bool fired;
    public bool hooked;
    public GameObject hookedObj;

    public float maxDistance;
    private float currentDistance;

    private bool grounded;

    private Vector3 defaultScale;
    private void Start()
    {
        defaultScale = hook.transform.localScale;
    }

    /// <summary>
    /// Update function for firing the hook, checking if the hook is connected to an object and moving the player if hooked
    /// </summary>
    void Update()
    {
        
        //Firing the hook
        if (Input.GetMouseButtonDown(0) && fired == false && Cursor.lockState == CursorLockMode.Locked)
            fired = true;

        //Draws rope from player to hooking point
        if (fired)
        {
            LineRenderer rope = hook.GetComponent<LineRenderer>();
            rope.SetVertexCount(2);
            rope.SetPosition(0, hookHolder.transform.position);
            rope.SetPosition(1, hook.transform.position);
        }
        
        //Fires hook forward until it hits max distance
        if (fired == true && hooked == false)
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
                ReturnHook();
        }

        //If the hook is connected to an object the player moves towards the hookpoint
        if (hooked == true && fired == true)
        {
            hook.transform.parent = hookedObj.transform;
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            this.GetComponent<Rigidbody>().useGravity = false;

            if (distanceToHook < 1.5)
            {
                ReturnHook();
            }
        }

        //While not fired
        else {
            hook.transform.parent = hookHolder.transform;
            hook.transform.localScale = defaultScale;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    /// <summary>
    /// Returns hook to the hook holder and resets the rope
    /// </summary>
    void ReturnHook()
    {
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.SetVertexCount(0);
    }

    /// <summary>
    /// Check if grounded
    /// </summary>
    void CheckGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            grounded = true;
        }
        else {
            grounded = false;
        }
    }

}
