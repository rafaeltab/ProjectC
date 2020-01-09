using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 playerStartPos;

    /// <summary>
    /// Make sure everything is setup and not active right now nothing goes wrong
    /// </summary>
    void Start()
    {
        playerStartPos = playerTransform.position;
        //Initialize all components
        GetComponentInChildren<Button>().onClick.AddListener(Respawn);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Kill the player
    /// </summary>
    public void Die()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Respawn the player
    /// </summary>
    public void Respawn()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gameObject.SetActive(false);
        Time.timeScale = 1;
        //TODO reset items
        playerTransform.position = playerStartPos;
        playerTransform.GetComponentInChildren<PlayerStats>().Health = playerTransform.GetComponentInChildren<PlayerStats>().maxHealth;
    }
}
