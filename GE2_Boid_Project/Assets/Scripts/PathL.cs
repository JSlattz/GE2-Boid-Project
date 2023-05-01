using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathL : MonoBehaviour
{
    public Transform[] waypoints;

    public bool isLooped = true;

    int current = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        for(int i = 1; i < waypoints.Length; i++)
        {
            Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            Gizmos.DrawSphere(waypoints[i - 1].position, 1);
            Gizmos.DrawSphere(waypoints[i].position, 1);
        }

        if(isLooped)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }

    public Vector3 Next()
    {
        return waypoints[current].position;
    }

    public bool IsLast()
    {
        return (current == waypoints.Length - 1);
    }

    public void AdvanceToNext()
    {
        if (!isLooped)
        {
            if (!IsLast())
            {
                current++;
            }
        }
        else
        {
            current = (current + 1) % waypoints.Length;
        }
    }
}
