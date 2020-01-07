using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public int count = 20;
    private List<GameObject> bats;
    public GameObject batPrefab;
    public float maxDistanceToObject = 30f;

    private bool checking = false;

    // Start is called before the first frame update
    void Start()
    {
        bats = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            bats.Add(Instantiate(batPrefab, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!checking)
        {
            StartCoroutine(DoCheck());
        }
    }

    IEnumerator DoCheck()
    {
        checking = true;
        foreach (var bat in bats)
        {
            if (Vector3.Distance(bat.transform.position,transform.position) > maxDistanceToObject)
            {
                bat.transform.position = transform.position + BatAI.RandomPointNear(transform.position, (int) maxDistanceToObject/2);
                bat.GetComponent<BatAI>().ReTarget();
            }
            yield return null;
        }
        checking = false;
    }
}
