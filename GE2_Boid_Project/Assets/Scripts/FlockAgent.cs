using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public int health = 25, damage = 5;
    public float shootCooldown = 1f, radius = 200f;
    public LayerMask enemyMask;
    float startShootCooldown;
    bool inRadius;
    public bool showGizmos;

    //public ParticleSystem explosion, blaster1, blaster2;

    GameObject closest;
    GameObject[] agents;

    Flock agentFlock;
    public Camera cam;
    CameraSwitch camSwitch;

    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;

    public Collider AgentCollider { get { return agentCollider; } }

    private void Awake()
    {
        camSwitch = GameObject.FindGameObjectWithTag("Manager").GetComponent<CameraSwitch>();
    }

    void Start()
    {
        agentCollider = GetComponent<Collider>();

        camSwitch.camsList.Add(cam);
        startShootCooldown = shootCooldown;
    }

    void Update()
    {
        FindEnemy();
        inRadius = Physics.CheckSphere(transform.position, radius, enemyMask);

        if(agentFlock.active)
        {
            if(inRadius)
            {
                
            }
        }

        if (!agentFlock.active)
        {
            transform.position += transform.forward * Time.deltaTime * 50f;
        }

        if (health <= 0)
            Die();
    }

    public void Initialise(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }

    public GameObject FindEnemy()
    {
        if (CompareTag("R"))
            agents = GameObject.FindGameObjectsWithTag("NE");

        if (CompareTag("NE"))
            agents = GameObject.FindGameObjectsWithTag("R");

        closest = null;

        float distance = Mathf.Infinity;

        Vector3 pos = transform.position;

        foreach (GameObject go in agents)
        {
            Vector3 diff = go.transform.position - pos;
            float currDistance = diff.sqrMagnitude;

            if(currDistance < distance)
            {
                closest = go;
                distance = currDistance;
            }
        }

        return closest;
    }

    /*public void Shoot(int dmg)
    {
        FlockAgent fa = closest.GetComponent<FlockAgent>();

        shootCooldown -= Time.deltaTime;

        if(shootCooldown <= 0f)
        {
            blaster1.Play();
            blaster2.Play();
            fa.health -= dmg;
            shootCooldown = startShootCooldown;
        }
    }*/

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        camSwitch.camsList.Remove(cam);

        if(cam.isActiveAndEnabled)
        {
            Debug.Log("a haa");
            camSwitch.ChangeCamera();
        }
    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
