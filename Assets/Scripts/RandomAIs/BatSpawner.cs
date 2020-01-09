using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public int count = 20;
    private List<GameObject> bats;
    public GameObject batPrefab;
    public float maxDistanceToObject = 30f;
    public Transform near;
    public bool DrawLines = false;
    private bool checking = false;

    /// <summary>
    /// Spawn a few bats 
    /// </summary>
    void Start()
    {
        bats = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            bats.Add(Instantiate(batPrefab, transform));
            bats[i].GetComponent<BatAI>().DrawLines = DrawLines;
        }
    }

    bool oldDrawLines = false;

    /// <summary>
    /// Put all bats back near the player if they're to far away
    /// </summary>
    void Update()
    {
        if(DrawLines != oldDrawLines)
        {
            foreach (var bat in bats)
            {
                bat.GetComponent<BatAI>().DrawLines = DrawLines;
            }
            oldDrawLines = DrawLines;
        }

        if (!checking)
        {
            StartCoroutine(DoCheck());
        }
    }

    /// <summary>
    /// Check all bats for their distance in a Coroutine for speed 
    /// </summary>
    /// <returns></returns>
    IEnumerator DoCheck()
    {
        checking = true;
        foreach (var bat in bats)
        {
            if (Vector3.Distance(bat.transform.position, near.position) > maxDistanceToObject)
            {
                bat.transform.position = bat.GetComponent<BatAI>().TryFindRespawn(near.position, (int)maxDistanceToObject / 2);
                bat.GetComponent<BatAI>().ReTarget();
            }
            yield return null;
        }
        checking = false;
    }
}
