using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySight : MonoBehaviour
{
    //creates variables
    public float fieldOfViewAngle = 110f;
    public bool detected;

    public SphereCollider col;
    public Animator anim;
    public GameObject player;

    public bool attackVoidUsed = false;

    //sets the audio stuff
    public AudioSource bgMusic;
    public AudioClip battleMusic;

    bool battleMusicPlaying = false;

    /// <summary>
    /// this is to make sure the enemy can detect the player each frame
    /// </summary>
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        //gives the angle from 2 vectors, in this case forward and direction
        float angle = Vector3.Angle(direction, transform.forward);

        //checks if angle is smaller than FoV / 2 
        if(angle < fieldOfViewAngle * 0.5f)
        {
            //makes raycast
            RaycastHit hit;
            /// <summary>
            /// checks if raycast hits something and checks if its player
            /// </summary>
            if(Physics.Raycast(transform.position + transform.up /2, direction.normalized, out hit, col.radius))
            {
                if(hit.collider.gameObject == player)
                {
                    detected = true;
                }else{
                    detected = false;
                    anim.SetBool("detected", false);
                }
            }
        }

        ///<summary>
        /// checks if the player is detected
        /// if so make the detected variable for the animtor true
        /// make the enemy look at and walk towards the player
        ///</summary>
        if(detected)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.02f);
            transform.LookAt(player.transform.position);

            //the bool "detected" for animation is set to true
            anim.SetBool("detected", true);

            //checks if the bool is false, otherwise the audio plays everyframe
            if(battleMusicPlaying == false)
            {
                playBattleMusicOn();
            }
        }
    }

    /// <summary>
    /// the attack void
    /// this is what happens when the enemy attacks
    /// </summary>
    void Attack(){
        anim.SetBool("attack", true);
        attackVoidUsed = true;
    }

    /// <summary>
    /// plays the battle music
    /// also makes the bool true so that this isnt called every frame
    /// </summary>
    void playBattleMusicOn()
    {
        bgMusic.clip = battleMusic;
        bgMusic.Play();
        battleMusicPlaying = true;
    }

}
