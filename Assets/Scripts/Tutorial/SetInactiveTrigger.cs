using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactiveTrigger : MonoBehaviour
{
    private bool done = false;
    public GameObject obj;

    //Set the given GameObject to inactive
    private void OnTriggerEnter(Collider other)
    {
        if (!done && other.gameObject.name == "Player")
        {
            obj.SetActive(false);
            done = true;
        }
    }
}
