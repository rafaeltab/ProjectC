using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{

    // Update is called once per frame
    /// <summary>
    /// Fire a ray and call destroy on the hit object
    /// </summary>
    void Update()
    {
      if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                
                Destroyable destroy = hit.transform.gameObject.GetComponent<Destroyable>();                

                if (destroy != null)
                {
                    destroy.Destroy(hit);
                    //StartCoroutine(StartDestroy(0,destroy,hit));
                }                
            }
        }

    }

    //IEnumerator StartDestroy(float duration,Destroyable destroy,RaycastHit hit)
    //{
    //    yield return new WaitForSeconds(duration);
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        destroy.Destroy(hit);
    //    }        
    //}
}
