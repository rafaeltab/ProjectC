using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BatAI : MonoBehaviour
{
    private Vector3 target;
    public bool DrawLines = false;
    
    /// <summary>
    /// Find a first position to go to
    /// </summary>
    void Start()
    {
        if (!TryFindPos(100))
        {
            target = transform.position;
        }
    }

    /// <summary>
    /// Get a new target
    /// </summary>
    public void ReTarget()
    {
        TryFindPos(50);
    }

    /// <summary>
    /// Move and find a new position if we arrived
    /// </summary>
    void Update()
    {
        if (transform.position == target)
        {
            TryFindPos(50);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 6f);
        transform.LookAt(target,Vector3.up);
    }

    /// <summary>
    /// Find a new position
    /// </summary>
    /// <param name="maxTries">maximum tries at finding a position</param>
    /// <returns>Whether or not a position was found</returns>
    public bool TryFindPos(int maxTries)
    {
        int trys = 0;
        while (trys < maxTries)
        {
            var point = RandomPointNear(transform.position, 10);
            if (OkPos(transform.position, point, 0.5f))
            {
                target = point;
                //Debug.Log(trys);
                trys = maxTries;
                
                return true;
            }

            trys++;
        }
        return false;
    }

    /// <summary>
    /// Find a respawn near a point
    /// </summary>
    /// <param name="near">The point to find a respawn near</param>
    /// <param name="distance">the maximum distance for the respawn</param>
    /// <returns></returns>
    public Vector3 TryFindRespawn(Vector3 near, int distance)
    {
        int trys = 0;
        while (trys < 100)
        {
            var point = RandomPointNear(near, distance);
            if (OkPos(near, point, 0.5f))
            {
                //Debug.Log(trys);
                trys = 100;

                return point;
            }

            trys++;
        }
        return near;
    }

    /// <summary>
    /// Draw the target of the bat
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target, 0.2f);
    }

    /// <summary>
    /// Check whether a position is good or not
    /// </summary>
    private bool OkPos(Vector3 loc, Vector3 t, float minDistanceFromWall)
    {
        var direction = loc - t;
        direction.Normalize();

        if (DrawLines)
        {
            drawALine(loc, -direction, Vector3.Distance(loc, t));
        }
        else
        {
            drawALine(Vector3.zero, Vector3.zero, 0);
        }
        

        RaycastHit hit;
        Physics.Raycast(loc, -direction,out hit,Vector3.Distance(loc,t) + minDistanceFromWall);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Draw a line from a point in a direction wit h alength
    /// </summary>
    public void drawALine(Vector3 pos, Vector3 dir, float length)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, pos);
        lineRenderer.SetPosition(1, dir * length + pos);
    }

    /// <summary>
    /// Find a random point near another point
    /// </summary>
    public static Vector3 RandomPointNear(Vector3 point, int distanceMax)
    {
        float xMin = point.x - distanceMax;
        float yMin = point.y - distanceMax;
        float zMin = point.z - distanceMax;

        float xMax = point.x + distanceMax;
        float yMax = point.y + distanceMax;
        float zMax = point.z + distanceMax;

        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);
        float z = Random.Range(zMin, zMax);

        return new Vector3(x, y, z);
    }
}
