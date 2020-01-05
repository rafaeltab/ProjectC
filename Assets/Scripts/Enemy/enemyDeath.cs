using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeath : MonoBehaviour
{
    public bool dead;

    GameObject player;
    public Animator anim;
    public bool deathVoidUsed = false;
    public float enemyHealth;

    float dist = 0f;
    bool startTimer = false;
    float decayTimer = 6f;

    void Start()
    {
        enemyHealth = 100;
        player = GameObject.FindWithTag("Player");
    }

    /// <summary>
    /// this is to make sure the enemy can detect the player each frame
    /// also checks if the enemy should despawn from distance or decay
    /// </summary>
    void Update()
    {
        if(enemyHealth <= 0 && deathVoidUsed == false)
        {
            Death();
        }

        dist = Vector3.Distance(player.transform.position, transform.position);

        if(dist >= 15f || decayTimer <= 0f)
        {
            Destroy(gameObject);
        }

        if(startTimer)
        {
            decayTimer -= Time.deltaTime;
        }
    }

     /// <summary>
    /// plays kill animation and starts decaytimer
    /// </summary>
    void Death()
    {
        anim.SetBool("dead", true);
        deathVoidUsed = true;
        startTimer = true;
    }

     /// <summary>
    /// gives damage
    /// </summary>
    void Damaged()
    {
        enemyHealth -= 20;
    }
}

