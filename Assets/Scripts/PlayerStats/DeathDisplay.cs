using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 playerStartPos;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPos = playerTransform.position;
        //Initialize all components
        GetComponentInChildren<Button>().onClick.AddListener(Respawn);
        gameObject.SetActive(false);
    }

    public void Die()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
    }

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
