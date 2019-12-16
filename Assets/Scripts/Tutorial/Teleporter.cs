using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public bool moveToScene;

    public int sceneBuildIndex;

    public int x;
    public int y;
    public int z;

    //Teleport player to location if touching collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (moveToScene) //Moves player to given scene
            {
                SceneManager.LoadScene(sceneBuildIndex);
            }
            else //Moves player to xyz location
            {
                MovementController.TeleportPlayer(x, y, z);
                GrapplingHook.ReturnHook();
            }
        }
    }
}
