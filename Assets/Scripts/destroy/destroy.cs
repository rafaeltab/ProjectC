using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    private Material highlightMaterial;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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
                    StartCoroutine(StartDestroy(1,destroy,hit));
                }
            }
        }

    }
    /// <summary>
    /// destroys an object on a timer basis, duration is how long it takes before something is destroyed, destroy is our custom destroy script
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="destroy"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    IEnumerator StartDestroy(float duration,Destroyable destroy,RaycastHit hit)
    {
        yield return new WaitForSeconds(duration);
        if (Input.GetMouseButtonDown(1))
        {
            destroy.Destroy(hit);
        }        
    }
}
