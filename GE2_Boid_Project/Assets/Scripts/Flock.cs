using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Spawning")]
    public Vector3 size;
    public float delay = 0.25f;
    int i = 0;

    [Space]
    public bool active = false;
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int startingCount = 250;
    const float agentDensity = 0.5f;    //how close agents spawn to each other

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 50f)]
    public float neighbourRadius = 1.5f;

    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed, squareNeighbourRadius, squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        if (i < startingCount)
            InvokeRepeating("Spawn", 0f, delay);
    }

    void Update()
    {
        if (i >= startingCount)
            CancelInvoke("Spawn");

        if(active)
        { 
            foreach(FlockAgent agent in agents)
            {
                if(agent != null)
                {
                    List<Transform> context = GetNearbyObjects(agent);
                    Vector3 move = behaviour.CalculateMove(agent, context, this);

                    move *= driveFactor;

                    if(move.sqrMagnitude > squareMaxSpeed)
                    {
                        move = move.normalized * maxSpeed;      //keep it at maxSpeed
                    }

                    agent.Move(move);
                }
            }
        }
    }

    void Spawn()
    {
        float x = Random.Range(-size.x / 2, size.x / 2);
        float y = Random.Range(-size.y / 2, size.y / 2);
        float z = Random.Range(0f, 360f);

        //Vector3 pos = Random.insideUnitSphere * startingCount * agentDensity;
        Vector3 pos = transform.position + new Vector3(x, y, 0f);

        /*Vector3 rotation = new Vector3(x, y, z);
        Quaternion rot = Quaternion.Euler(rotation);*/

        FlockAgent newAgent = Instantiate(agentPrefab, pos, transform.rotation, transform);
        newAgent.name = "Agent " + i;
        newAgent.Initialise(this);
        agents.Add(newAgent);
        i++;
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);

        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider)    //if c is not the collider of this game object
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, size);
    }
}
