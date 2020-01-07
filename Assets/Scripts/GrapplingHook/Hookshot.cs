using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
    [SerializeField] private Transform hookTransform;
    [SerializeField] private Transform gunTransform;

    private CharacterController characterController;
    private MovementController movementController;
    public Camera playerCamera;
    private float cameraVertAngle;
    private float charVelocityY;
    private Vector3 charVelocityMomentum;
    private State state;
    private Vector3 grapplePoint;
    public GameObject grapplingHook;

    private enum State
    {
        Normal,
        GrappleShot,
        GrappleFlying
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<MovementController>();
        state = State.Normal;
    }

    private void Update()
    {
        if (grapplingHook.activeSelf)
        {
            switch (state)
            {
                default:
                case State.Normal:
                    HandleGrappleStart();
                    break;
                case State.GrappleShot:
                    HandleGrappleShot();
                    break;
                case State.GrappleFlying:
                    HandleGrappleMovement();
                    break;
            }
        }
        else { ResetStateNormal(); }
    }

    private void HandleGrappleStart()
    {
        if (InputDownGrapple() && Cursor.lockState == CursorLockMode.Locked)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, 200f))
            {
                if (raycastHit.collider.gameObject.tag == "Hookable")
                {

                    //Hit object
                    //hookTransform.position = raycastHit.point;
                    grapplePoint = raycastHit.point;
                    hookTransform.SetParent(null, true);
                    state = State.GrappleShot;

                }
                else
                {
                    Debug.Log(raycastHit.collider.gameObject.tag);
                }
            }
        }
    }

    private void HandleGrappleShot()
    {
        LineRenderer rope = hookTransform.GetComponent<LineRenderer>();
        rope.SetVertexCount(2);
        rope.SetPosition(0, gunTransform.transform.position);
        rope.SetPosition(1, hookTransform.transform.position);

        hookTransform.position = Vector3.MoveTowards(hookTransform.position, grapplePoint, Time.deltaTime * 40f);

        if (hookTransform.position == grapplePoint)
        {
            movementController.enabled = false;
            characterController.enabled = true;
            state = State.GrappleFlying;
        }

        if (InputDownGrapple())
        {
            //Cancel grapple
            rope.SetVertexCount(0);
            ResetStateNormal();
        }

        if (InputDownJump())
        {
            //Cancel grapple by jumping, more movement
            rope.SetVertexCount(0);
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0), ForceMode.Impulse);
            ResetStateNormal();
        }
    }

    private void HandleGrappleMovement()
    {
        LineRenderer ropeReset = hookTransform.GetComponent<LineRenderer>();
        Vector3 grappleDir = (grapplePoint - transform.position).normalized;

        float grappleSpeedMin = 10f;
        float grappleSpeedMax = 30f;
        float grappleSpeed = Mathf.Clamp(Vector3.Distance(transform.position, grapplePoint), grappleSpeedMin, grappleSpeedMax);
        float grappleSpeedMultipl = 1.5f;

        //Move char
        characterController.Move(grappleDir * grappleSpeed * grappleSpeedMultipl * Time.deltaTime);

        float reachedGrapplePoint = 1.3f;
        if (Vector3.Distance(transform.position, grapplePoint) < reachedGrapplePoint)
        {
            //Reached grapple pos
            ropeReset.SetVertexCount(0);
            ResetStateNormal();
        }

        if (InputDownGrapple())
        {
            //Cancel grapple
            ropeReset.SetVertexCount(0);
            ResetStateNormal();
        }

        if (InputDownJump())
        {
            //Cancel grapple by jumping, more movement
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 15, 0), ForceMode.Force);
            ropeReset.SetVertexCount(0);
            ResetStateNormal();
        }
    }

    private bool InputDownGrapple()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private bool InputDownJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void ResetStateNormal()
    {
        state = State.Normal;
        movementController.enabled = true;
        characterController.enabled = false;
        hookTransform.SetParent(gunTransform, true);
        hookTransform.position = gunTransform.position;
        hookTransform.rotation = gunTransform.rotation;
    }
}
