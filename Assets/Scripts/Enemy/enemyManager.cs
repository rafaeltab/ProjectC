using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
 {
    public float spawnTime = 5f;
    //The amount of time between each spawn.
    public float spawnDelay = 3f;
    //The amount of time before spawning starts.
    public GameObject[] enemyCount;
    //Array of enemy prefabs.
    public Vector3 enposition;

    public GameObject player;
    public GameObject enemyPrefab;
    public Vector3 playerPos;

    void Start ()
    {
        //Start calling the Spawn function repeatedly after a delay.
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }
    void Spawn ()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        //Instantiate a random enemy.
        if(enemyCount.Length <2)
        {
            playerPos = player.transform.position;
            enposition = new Vector3(Random.Range(playerPos.x-10, playerPos.x+10), playerPos.y, Random.Range(playerPos.z-10, playerPos.z+10));
            Instantiate(enemyPrefab, enposition, transform.rotation);
        }
        
    }
}
