using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public enum DestroyType
    {
        DESTROY,
        SCALE
    }

    public DestroyType destroyFunc;
    /// <summary>
    /// test if a raycast hits a gameobject and tries to destroy or scale it up, depending on the set behavior. parameter is the raycast
    /// </summary>
    /// <param name="hit"></param>
    public void Destroy(RaycastHit hit)
    {
        switch (destroyFunc)
        {
            case DestroyType.DESTROY:  
                Destroy(hit.transform.gameObject);
                break;
            case DestroyType.SCALE:
                hit.transform.localScale *= 2;
                break;
        }
    }
}