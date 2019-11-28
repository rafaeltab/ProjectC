using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    private Material highlightMaterial;
    public Camera mainCam;

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(1))
        {
            
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2,0));
            Debug.Log(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                Destroyable destroy = hit.transform.gameObject.GetComponent<Destroyable>();

                if (destroy != null)
                {
                    StartCoroutine(StartDestroy(0,destroy,hit));
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
