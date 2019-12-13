using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeath : MonoBehaviour
{
    public bool dead;

    public GameObject player;
    public Animator anim;
    public bool deathVoidUsed = false;
    public static float enemyHealth;

    void Start()
    {
        enemyHealth = 100;
    }

    /// <summary>
    /// this is to make sure the enemy can detect the player each frame
    /// </summary>
    void Update()
    {
        if(enemyHealth <= 0 && deathVoidUsed == false)
        {
            Death();
        }
    }

    void Death(){
        anim.SetBool("dead", true);
        deathVoidUsed = true;
    }
}

