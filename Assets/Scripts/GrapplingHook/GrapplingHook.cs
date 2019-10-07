using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grappling_Hook
{
    public class GrapplingHook : MonoBehaviour
    {
        public float maxGrappleDist = 1;
        public Camera mainCam;
        private bool grappled = false;
        private Vector3 grappleLocation;
        public float force = 5;
        public Rigidbody body;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Grapple();
            }

            else if (Input.GetMouseButtonUp(0))
            {
                grappled = false;
            }

            if (grappled)
            {
                body.AddForce(LookAt(transform.position, grappleLocation) * force, ForceMode.Force);
            }
        }

        private Vector3 LookAt(Vector3 current, Vector3 target)
        {
            Vector3 direction = (target - current) / Vector3.Distance(current, target);
            return direction;
        }

        private void Grapple()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, maxGrappleDist))
            {
                grappled = true;
                Debug.Log(hit.transform.name);
                grappleLocation = hit.point;
            }
        }
    }
}
