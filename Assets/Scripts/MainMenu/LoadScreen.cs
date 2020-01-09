using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the loading screen
/// </summary>
public class LoadScreen : MonoBehaviour
{
    public Rigidbody player;
    public  MovementController movementController;
    public GameObject loadScreen;
    private float timeElapsed;

    /// <summary>
    /// Sets time elapsed to 0
    /// </summary>
    private void Start()
    {
        timeElapsed = 0f;
    }

    /// <summary>
    /// Check if the player is still falling, if the players velocity is under 0.1 the loading screen goes away
    /// </summary>
    private void Update()
    {
        if (timeElapsed > 1)
        {
            //Check if velocity = 0
            if (player != null)
            {
                if (Vector3.Distance(player.velocity, Vector3.zero) <= 0.1f)
                {
                    if (loadScreen != null)
                    {
                        loadScreen.SetActive(false);
                        movementController.enabled = true;
                        PauseMenu2.loading = false;
                    }
                }
                else
                {
                    movementController.enabled = false;
                }
            }
        }
        timeElapsed += Time.deltaTime;
    }
}
