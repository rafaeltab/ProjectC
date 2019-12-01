using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject red;
    public GameObject blue;
    int activeNr = 0;
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I) | Input.GetKeyDown(KeyCode.J))
        {
            activeNr++;
            if (activeNr == 2)
            {
                activeNr = 0;
            }

            red.SetActive(activeNr == 0);
            blue.SetActive(activeNr == 1);       
            
        }
    }
}