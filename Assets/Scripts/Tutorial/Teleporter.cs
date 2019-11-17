using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public int x;
    public int y;
    public int z;

    //Teleport player to xyz location if touching collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") {
            MovementController.TeleportPlayer(x, y, z);
            GrapplingHook.ReturnHook();
        }
    }
}
