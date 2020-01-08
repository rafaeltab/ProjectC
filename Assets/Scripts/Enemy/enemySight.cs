using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySight : MonoBehaviour
{
    //creates variables
    public float fieldOfViewAngle = 110f;
    public bool detected;

    public SphereCollider col;
    public BoxCollider hitbox;
    public Animator anim;
    GameObject player;

    bool attackVoidUsed = false;
    bool oORUsed = false;
    bool kickCooldown = false;

    //sets the audio stuff
    public AudioSource audioMusicPlayer;
    public AudioClip battleMusic;

    bool battleMusicPlaying = false;


    public UnityEngine.AI.NavMeshAgent agent;

    float dist = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        audioMusicPlayer = player.transform.Find("Music Player").GetComponent<AudioSource>();
    }

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
                    anim.SetBool("backToIdle", true);
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
            Vector3 targetLocation = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            //agent.SetDestination(targetLocation);
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, 0.03f);
            transform.LookAt(targetLocation);

            dist = Vector3.Distance(player.transform.position, transform.position);

            //the bool "detected" for animation is set to true
            anim.SetBool("detected", true);
            anim.SetBool("backToIdle", false);

            //checks if the bool is false, otherwise the audio plays everyframe
            if(battleMusicPlaying == false)
            {
                playBattleMusicOn();
            }



            if(dist <= 2f && attackVoidUsed == false)
            {
                Attack();
            }

            if(dist >= 2f && oORUsed == false)
            {
                outOfRange();
            }



            if(dist <= 2f && anim.GetCurrentAnimatorStateInfo(0).IsName("kick") && kickCooldown == false)
            {
                kickCooldown = true;
                player.GetComponent<PlayerStats>().Health =  player.GetComponent<PlayerStats>().Health - 25;
                print(player.GetComponent<PlayerStats>().Health);
            }

            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("kick"))
            {
                kickCooldown = false;
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
        oORUsed = false;
    }
    void outOfRange(){
        anim.SetBool("attack", false);
        attackVoidUsed = false;
        oORUsed = true;
    }

    /// <summary>
    /// plays the battle music
    /// also makes the bool true so that this isnt called every frame
    /// </summary>
    void playBattleMusicOn()
    {
        if(audioMusicPlayer.clip != battleMusic && battleMusicPlaying == false)
        {
            audioMusicPlayer.clip = battleMusic;
            audioMusicPlayer.Play();
            battleMusicPlaying = true;
        }
        
    }

}
