using System.Collections;
using UnityEngine;


public class HunterScript : MonoBehaviour
{

    public float speed = 5;
    public float walkZone = 100;
    public bool debug;

    public float huntingZone = 50;
    public LayerMask Fishlayer;
    public Material sharkMaterial;

    Transform thisTransform;
    Vector3 vel, velCam, target, targetCurent, rndTarget, targetFish;
    float startYpos, huntTime, huntSpeed, speedSh, acselSh;
    public int hunger = 100;
    bool hunting;
    static WaitForSeconds delay0 = new WaitForSeconds(8.0f);


    void Awake()
    {
        Quaternion thisRotation = Quaternion.Euler(0, 0, -90);
        gameObject.transform.rotation = thisRotation;
        thisTransform = transform;
        startYpos = transform.position.y;
        huntSpeed = 1.0f;

        StartCoroutine(RandomVector());

    }


    void LateUpdate()
    {

        Hunting();
        Move();

    }


    void Hunting()
    {

        if (hunting == false)
        {
            targetCurent = rndTarget;
            if (hunger > 0)
            {
                hunger -= 1;
            }
            else if (Physics.CheckSphere(thisTransform.position, huntingZone, Fishlayer))
            {
                hunting = true;
            }

            if (huntSpeed > 1.0f)
            {
                huntSpeed -= Time.deltaTime * 0.2f;
            }

            if (acselSh > 0)
            {
                acselSh -= Time.deltaTime * 0.1f;
            }
        }
        else
        {
            if (hunger < 50)
            {
                hunting = true;
            }
            else
            {
                hunting = false;
            }

            Collider[] fishColliders = Physics.OverlapSphere(thisTransform.position, huntingZone, Fishlayer);

            if (fishColliders.Length > 0)
            {
                targetFish = fishColliders[0].transform.position;
                targetCurent = targetFish;
            }

            if (huntSpeed < 2.1f)
            {
                huntSpeed += Time.deltaTime * 0.2f;
            }

            if (acselSh < 0.6f)
            {
                acselSh += Time.deltaTime * 0.1f;
            }
        }

        if (acselSh >= 0)
        {
            speedSh += Time.deltaTime * acselSh;
            sharkMaterial.SetFloat("_ScriptControl", speedSh);
        }

    }


    void Move()
    {

        target = Vector3.SmoothDamp(target, targetCurent, ref vel, 3.0f);
        target = new Vector3(target.x, startYpos, target.y);
        Vector3 newDir = Vector3.RotateTowards(thisTransform.forward, target - thisTransform.position, Time.deltaTime * 0.35f, 0);
        thisTransform.rotation = Quaternion.LookRotation(newDir);
        thisTransform.Translate(Vector3.forward * Time.deltaTime * huntSpeed * speed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fish" && hunger < 100)
        {
            hunger += 50;
            Destroy(collision.gameObject);
            Debug.Log("collision");
        }

    }

    IEnumerator RandomVector()
    {

        while (true)
        {
            rndTarget = Random.insideUnitSphere * walkZone;
            yield return delay0;
        }
    }
}
