using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidL : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 5;
    public float maxForce = 10;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;

    public float mass = 1;

    [Space]
    [Header("Seek")]

    public bool seekEnabled = true;
    public Transform seekTargetTransform;
    public Vector3 seekTarget;

    [Space]
    [Header("Arrive")]

    public bool arriveEnabled = false;
    public Transform arriveTargetTransform;
    public Vector3 arriveTarget;
    public float slowingDistance = 80;

    [Space]
    [Header("Path")]

    public Path path;
    public bool pathFollowingEnabled = false;
    public float waypointDistance = 3;

    [Space]
    [Header("Banking")]

    public float banking = 0.1f;
    public float damping = 0.1f;

    [Space]
    [Header("Player Steering")]

    public bool playerSteeringEnabled = false;
    public float steeringForce = 100;

    [Space]
    [Header("Pursue")]

    public bool pursueEnabled = false;
    public Boid pursueTarget;

    public Vector3 pursueTargetPos;

    [Space]
    [Header("Offset Pursue")]

    public bool offsetPursueEnabled = false;
    public Boid leader;
    public Vector3 offset;
    private Vector3 worldTarget;
    private Vector3 targetPos;

    void Start()
    {
        
    }

    void Update()
    {
        force = CalculateForce();
        acceleration = force / mass;
        velocity = velocity + acceleration * Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
        speed = velocity.magnitude;
        if (speed > 0)
        {
            //transform.forward = velocity;
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + velocity, tempUp);

            //velocity *= 0.9f;

            // Remove 10% of the velocity every second
            velocity -= (damping * velocity * Time.deltaTime);
        }
    }

    public Vector3 PathFollow()
    {
        Vector3 nextWaypoint = path.Next();
        if (!path.isLooped && path.IsLast())
        {
            return Arrive(nextWaypoint);
        }
        else
        {
            if (Vector3.Distance(transform.position, nextWaypoint) < waypointDistance)
            {
                path.AdvanceToNext();
            }
            return Seek(nextWaypoint);
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return (desired - velocity);
    }

    public Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;
        float ramped = (dist / slowingDistance) * maxSpeed;
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / dist);
        return desired - velocity;
    }

    public Vector3 Pursue(Boid pursueTarget)
    {
        float dist = Vector3.Distance(pursueTarget.transform.position, transform.position);
        float time = dist / maxSpeed;
        pursueTargetPos = pursueTarget.transform.position + pursueTarget.velocity * time;
        return Seek(pursueTargetPos);
    }

    public Vector3 OffsetPursue(Boid leader)
    {
        worldTarget = leader.transform.TransformPoint(offset);
        float dist = Vector3.Distance(transform.position, worldTarget);
        float time = dist / maxSpeed;

        targetPos = worldTarget + (leader.velocity * time);
        return Arrive(targetPos);
    }

    public Vector3 PlayerSteering()
    {
        Vector3 force = Vector3.zero;
        force += Input.GetAxis("Vertical") * transform.forward * steeringForce;

        Vector3 projected = transform.right;
        projected.y = 0;
        projected.Normalize();

        force += Input.GetAxis("Horizontal") * projected * steeringForce;

        // Put your code here!
        return force;
    }

    public Vector3 CalculateForce()
    {
        Vector3 f = Vector3.zero;
        if (seekEnabled)
        {
            if (seekTargetTransform != null)
            {
                seekTarget = seekTargetTransform.position;
            }
            f += Seek(seekTarget);
        }

        if (arriveEnabled)
        {
            if (arriveTargetTransform != null)
            {
                arriveTarget = arriveTargetTransform.position;
            }
            f += Arrive(arriveTarget);
        }

        if (pathFollowingEnabled)
        {
            f += PathFollow();
        }

        if (playerSteeringEnabled)
        {
            f += PlayerSteering();
        }

        if (pursueEnabled)
        {
            f += Pursue(pursueTarget);
        }

        if (offsetPursueEnabled)
        {
            f += OffsetPursue(leader);
        }

        return f;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);

        if (arriveEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(arriveTargetTransform.position, slowingDistance);
        }

        if (pursueEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, pursueTargetPos);
        }

    }
}
