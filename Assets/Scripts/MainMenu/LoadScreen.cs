using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{
    public Rigidbody player;
    public  MovementController movementController;
    public GameObject loadScreen;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;
    }

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
