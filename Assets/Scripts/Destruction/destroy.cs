using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{

    // Update is called once per frame
    /// <summary>
    /// Fire a ray and upon hit will call the game objects destroyable script.
    /// </summary>
    void Update()
    {
      if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                
                Destroyable destroy = hit.transform.gameObject.GetComponent<Destroyable>();                

                if (destroy != null)
                {
                    destroy.Destroy(hit);
                    
                }                
            }
        }

    }
}
