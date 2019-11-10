using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        /// <summary>
        /// Checks for the tag "Hookable", if there is no tag it won't hook
        /// </summary>
        /// <param name="other"></param>
        if (other.tag == "Hookable")
        {
            player.GetComponent<GrapplingHook>().hooked = true;
            player.GetComponent<GrapplingHook>().hookedObj = other.gameObject;
        }
    }
}
