using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
    public GameObject targetGO;
    public Boid target;

    Vector3 targetPos;
    public void Start()
    {
       targetGO = GameObject.FindWithTag("Leader");
        target = targetGO.GetComponent<Boid>();
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying && isActiveAndEnabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

    public override Vector3 Calculate()
    {
        float dist = Vector3.Distance(target.transform.position, transform.position);
        float time = dist / boid.maxSpeed;

        targetPos = target.transform.position + (boid.velocity * time);

        return boid.SeekForce(targetPos);
    }
}
