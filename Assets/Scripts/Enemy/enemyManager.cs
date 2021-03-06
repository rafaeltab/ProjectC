﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
 {
    float spawnTime = 15f;
    //The amount of time between each spawn.
    float spawnDelay = 0.3f;
    //The amount of time before spawning starts.
    public GameObject[] enemyCount;
    //Array of enemy prefabs.
    public Vector3 enposition;

    public GameObject player;
    public GameObject enemyPrefab;
    public Vector3 playerPos;
    public Music musicInstance;

    void Start ()
    {
        //Start calling the Spawn function repeatedly after a delay.
        InvokeRepeating("Spawn", spawnDelay, spawnTime);
        player = GameObject.FindWithTag("Player");
        musicInstance = Music.instance;
    }
    void Update()
    {
        //checks if no enemies and stops music
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyCount.Length <=0 && musicInstance._as.clip.name == "BattleMusic")
        {
            Debug.Log("Stop");
            musicInstance.PlayMusic();
        }
    }
    void Spawn ()
    {
        //Instantiate a random enemy.
        if(enemyCount.Length <2)
        {
            playerPos = player.transform.position;
            enposition = new Vector3(Random.Range(playerPos.x-10, playerPos.x+10), playerPos.y, Random.Range(playerPos.z-10, playerPos.z+10));
            Instantiate(enemyPrefab, enposition, transform.rotation);
        }
    }
}
