using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public bool detected;

    public UnityEngine.AI.NavMeshAgent nav;
    public SphereCollider col;
    public Animator anim;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        
        if(angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position + transform.up /2, direction.normalized, out hit, col.radius))
            {
                if(hit.collider.gameObject == player)
                {
                    detected = true;
                    print(detected);
                }else{
                    detected = false;
                }
            }
        }
    }
}
