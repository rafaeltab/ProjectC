using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeath : MonoBehaviour
{
    public GameObject hitEffect;

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
        //starts Death() if enemy has no health and not in use already
        if(enemyHealth <= 0 && deathVoidUsed == false)
        {
            Death();
        }

        dist = Vector3.Distance(player.transform.position, transform.position);
        //checks if out of range or the decaytimer is 0 to destroy the enemy so a new one can spawn
        if(dist >= 15f || decayTimer <= 0f)
        {
            Destroy(gameObject);
        }
        //lowers the decaytimer
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
        Destroy(GetComponent<enemySight>());
    }

     /// <summary>
    /// gives damage
    /// </summary>
     public void Damaged()
    {
        enemyHealth -= 20;
    }

    public void DamagedWithHitEffect(RaycastHit hit)
    {
        Damaged();
        //Create a hit effect
        if (hitEffect != null)
        {
            //Create hitEffect and remove after 5 seconds
            Destroy(Instantiate(hitEffect, hit.point, Quaternion.Euler(hit.normal), hit.transform),5);
        }
    }
}

