using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Destroyable : MonoBehaviour
{
    public enum DestroyType
    {
        DESTROY,
        SCALE,
        ENEMYDAMAGE
    }

    public DestroyType destroyFunc;
    
    /// <summary>
    /// Use the correct way to destroy this object
    /// </summary>
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
                case DestroyType.ENEMYDAMAGE:
                    hit.collider.GetComponent<enemyDeath>().DamagedWithHitEffect(hit);
                    break;
            }
        }catch(Exception e){

        }
    }
}