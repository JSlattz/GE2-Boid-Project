using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    PlantSpawnerScript plantSpawnerScript; PlantScript plantScript;
    public GameObject seed; public GameObject prefab;
    public Vector3 currentPos;
    bool seedReady = false; 
    // Start is called before the first frame update
    void Awake()
    {
        seed = this.gameObject;
        prefab = GameObject.Find("Base Plant");
        StartCoroutine("seedTimer");
        Debug.Log("Coroutine started");
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);  
    }

    private void OnCollisionEnter(Collision other)
    {
        if(seedReady == true)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            //prefab.name = "Base Plant" + plantScript.seedTracker;
            MeshRenderer meshRenderer = seed.GetComponent<MeshRenderer>();
            //meshRenderer.material.color = new Color(0f, 1f, 0.1600f);
            Instantiate(prefab, currentPos, rotation);
            prefab.GetComponent<PlantScript>().enabled = true;
            Destroy(this.gameObject);
        }
    }

    IEnumerator seedTimer()
    {
        Debug.Log("Timer Started");
        yield return new WaitForSeconds(20f);
        seed.GetComponent<Rigidbody>().mass = 0.1f; seed.GetComponent<Rigidbody>().useGravity = true;
        seed.GetComponent<Boid>().enabled = false;
        seed.GetComponent<NoiseWander>().enabled = false;
        seedReady = true;
    }
}
