using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("SHOT");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                
                Destroyable destroy = hit.transform.gameObject.GetComponent<Destroyable>();
                enemyDeath damage = hit.transform.gameObject.GetComponent<enemyDeath>();

                

                if (destroy != null)
                {
                    Debug.Log("destroys");
                    StartCoroutine(StartDestroy(0,destroy,hit));
                }
                if (damage != null)
                {
                    Debug.Log("hits");
                    damage.Damaged();
                }
            }
        }

    }

    IEnumerator StartDestroy(float duration,Destroyable destroy,RaycastHit hit)
    {
        yield return new WaitForSeconds(duration);
        if (Input.GetMouseButtonDown(1))
        {
            destroy.Destroy(hit);
        }        
    }
}
