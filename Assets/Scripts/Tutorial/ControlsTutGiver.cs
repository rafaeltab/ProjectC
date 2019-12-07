using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutGiver : MonoBehaviour
{
    private bool done = false;
    public int amountTasks;

    //Give player controls tutorial if touching collider
    private void OnTriggerEnter(Collider other)
    {
        if (!done && other.gameObject.name == "Player")
        {
            TutorialManager.NextTutorial(amountTasks);
            done = true;
        }
    }
}
