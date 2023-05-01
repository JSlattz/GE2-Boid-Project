using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawnerScript : MonoBehaviour
{
    public GameObject prefab; public GameObject seedPrefab; public GameObject basePlantPrefab;

    [Range(1, 5000)]
    public int radius = 50;

    public int count = 10;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < count; i++)
        {
            if(i == 0)
            {
                prefab.GetComponent<PlantScript>().enabled = false;
            }
            else
            {
                prefab.GetComponent<PlantScript>().enabled = true;
            }
            //prefab.SetActive(true);
            
            float rnd = Random.Range(0f, 360f);
            Vector3 pos = new Vector3(Random.insideUnitSphere.x * radius, -31.6f, Random.insideUnitSphere.z * radius);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Instantiate(prefab, pos, rotation);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
