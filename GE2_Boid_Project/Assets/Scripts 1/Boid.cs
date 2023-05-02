using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();
    NematodeSchool nematodeSchool;
    Nematode nematode;

    public Vector3 force = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public float mass = 1;
    public int hunger = 49;

    [Range(0.0f, 10.0f)]
    public float damping = 0.01f;

    [Range(0.0f, 1.0f)]
    public float banking = 0.1f;
    public float maxSpeed = 5.0f;
    public float maxForce = 10.0f;

    [Space]
    [Header("Pregnacy")]
    public bool pregnant = false; public bool checkingPregnancy = false; public bool gestating; public bool baby = false;
    public GameObject prefab;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);
    }

    // Use this for initialization
    void Start()
    {
        if(gameObject.tag != "Seed")
        {
            StartCoroutine("hungerCountdown");
        }
        
        prefab = GameObject.Find("Baby Fish Head");

        SteeringBehaviour[] behaviours = GetComponents<SteeringBehaviour>();
        foreach (SteeringBehaviour b in behaviours)
        {
            this.behaviours.Add(b);            
        }
    }

    public Vector3 SeekForce(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        return desired - velocity;
    }

    public Vector3 ArriveForce(Vector3 target, float slowingDistance = 10.0f,float  decelleration = 3)
    {
        Vector3 toTarget = target - transform.position;

        float distance = toTarget.magnitude;
        Vector3 desired;
        if (distance < slowingDistance)
        {
            desired = maxSpeed * (distance / slowingDistance) * (toTarget / distance);
        }
        else
        {
            desired = maxSpeed * (toTarget / distance);
            decelleration = 1;
        }    

        return desired - velocity * decelleration;
    }
    

    Vector3 Calculate()
    {
        force = Vector3.zero;

        // Weighted prioritised truncated running sum
        // 1. Behaviours are weighted
        // 2. Behaviours are prioritised
        // 3. Truncated
        // 4. Running sum

        foreach(SteeringBehaviour b in behaviours)
        {
            if (b.isActiveAndEnabled)
            {
                force += b.Calculate() * b.weight;
                float f = force.magnitude;
                if (f > maxForce)
                {
                    force = Vector3.ClampMagnitude(force, maxForce);
                    break;
                }
            }
        }

        

        return force;
    }

    IEnumerator hungerCountdown()
    {
        yield return new WaitForSeconds(5f);
        hunger--;
        if(hunger < 50)
        {
            this.GetComponent<Seek>().enabled = true;
        }
        else if(hunger >= 50)
        {
            this.GetComponent<Seek>().enabled = false;
        }
        StartCoroutine("hungerCountdown");
    }

    IEnumerator pregnancyChecker()
    {
        checkingPregnancy = true;
        yield return new WaitForSeconds(10f);
        Debug.Log("Checking Pregnancy");
        int rndPregnancy = Random.Range(1, 5);
        if(rndPregnancy == 4)
        {
            pregnant = true;
            checkingPregnancy = false;
            Debug.Log("Pegnant!");
        }
        else
        {
            checkingPregnancy = false;
        }
    }

    IEnumerator pregnancy()
    {
        pregnant = false;
        gestating = true;
        Debug.Log("stage begun");
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.HSVToRGB(0f, 1f, 1f);
        int i = 0;
        if (i < 10)
        {
            Vector3 pregnacyGrowth = new Vector3(0.01f, 0.01f, 0.01f);
            gameObject.GetComponentInParent<Transform>().localScale += pregnacyGrowth;
            yield return new WaitForSeconds(30f);
            i++;
            Debug.Log("stage complete");
            Debug.Log(i);
        }
        gestating = false; baby = true;
        meshRenderer.material.color = Color.HSVToRGB(0f, 0f, 1f);
        prefab.AddComponent<Nematode>(); prefab.AddComponent<SpineAnimator>();
        prefab.GetComponent<Nematode>().enabled = true; prefab.GetComponent<SpineAnimator>().enabled = true; prefab.GetComponent<SphereCollider>().enabled = true;
        Instantiate(prefab, transform.position, transform.rotation);
        Debug.Log("pregnancy done");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        force = Calculate();
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        
        if (velocity.magnitude > 0)
        {
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + velocity, tempUp);

            transform.position += velocity * Time.deltaTime;
            velocity *= (1.0f - (damping * Time.deltaTime));
        }

        pregnancyHandler();

        if(hunger > 100)
        {
            hunger = 100;
        }
        if(hunger == 100 && pregnant == false && checkingPregnancy == false && gestating == false)
        {
            StartCoroutine("pregnancyChecker");
        }
    }

    void pregnancyHandler()
    {
        if(pregnant == true && gestating == false)
        {
            StartCoroutine("pregnancy");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Food" && hunger < 100)
        {
            hunger += 25;
            Destroy(collision.gameObject);
            Debug.Log("collision");
        }

    }
}
