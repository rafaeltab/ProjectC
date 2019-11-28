using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destroyable : MonoBehaviour
{
    public enum DestroyType
    {
        DESTROY,
        SCALE
    }

    public DestroyType destroyFunc;
    
    public void Destroy(RaycastHit hit)
    {
        try
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
        }catch(Exception e){

        }
    }
}