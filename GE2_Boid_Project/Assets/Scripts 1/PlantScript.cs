using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour
{
    public List<GameObject> plantStems = new List<GameObject>();
    public int plantHeightMin; public int plantHeightMax; public int plantHeight; int rndPlantHeight;
    public GameObject firstPlantStem;
    bool plantCanGrow = true; bool seedsReady = true;
    int i; public int seedTracker; public int j;
    PlantSpawnerScript plantSpawnerScript;
    // Start is called before the first frame update
    void Start()
    {
        rndPlantHeight = Random.Range(plantHeightMin, plantHeightMax);
        plantHeight = rndPlantHeight;
        i = 0;
        j = plantStems.Count;
        if( j > 1)
        {
            Destroy(plantStems[j]);
            j--;
        }
        //seedPrefab = plantSpawnerScript.seedPrefab;
        //StartCoroutine("PlantGrowthTimer");
    }

    // Update is called once per frame
    void Update()
    {
        PlantGrow();
    }

    void PlantGrow()
    {
        if (plantCanGrow == true)
        {
            if (i < rndPlantHeight)
            {
                float height = 1f / rndPlantHeight * i;

                firstPlantStem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                MeshRenderer meshRenderer = firstPlantStem.GetComponent<MeshRenderer>();
                /*if (i == 0)
                {
                    firstPlantStem.AddComponent<Boid>();
                    firstPlantStem.AddComponent<ObstacleAvoidance>();
                    firstPlantStem.AddComponent<NoiseWander>();
                    firstPlantStem.AddComponent<Constrain>();

                }*/

                firstPlantStem.transform.position = new Vector3(0f, i + 0.5f, 0f);
                meshRenderer.material.color = new Color(0f, 1f, 0.1600f);
                firstPlantStem.transform.SetParent(transform);
                firstPlantStem.transform.localPosition = new Vector3(0f, i * 1f, 0f);
                plantStems.Add(firstPlantStem);
                j = plantStems.Count;
                StartCoroutine("PlantGrowthTimer");
                plantCanGrow = false;
                i++;
            }

            if (i == rndPlantHeight && seedsReady == true)
            {
                float height = 1f / rndPlantHeight * i;
                firstPlantStem = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                MeshRenderer meshRenderer = firstPlantStem.GetComponent<MeshRenderer>();
                firstPlantStem.AddComponent<Boid>();
                firstPlantStem.AddComponent<NoiseWander>();
                firstPlantStem.AddComponent<SeedScript>(); firstPlantStem.GetComponent<SeedScript>().enabled = true;
                firstPlantStem.AddComponent<Rigidbody>();
                firstPlantStem.GetComponent<Rigidbody>().mass = 0f; firstPlantStem.GetComponent<Rigidbody>().useGravity = false;
                firstPlantStem.transform.position = new Vector3(0f, i + 1f, 0f);
                meshRenderer.material.color = new Color(1f, 0f, 0.1600f);
                firstPlantStem.transform.SetParent(transform);
                firstPlantStem.transform.localPosition = new Vector3(0f, i * 1f, 0f);
                seedTracker++;
                //firstPlantStem.name = "Seed";
                plantCanGrow = false;
                seedsReady = false;
                StartCoroutine("SeedSpawnTimer");
            }
        }
    }

    IEnumerator PlantGrowthTimer()
    {
        yield return new WaitForSeconds(10f);
        plantCanGrow = true;
        StopCoroutine("PlantGrowthTimer");
    }

    IEnumerator SeedSpawnTimer()
    {
        yield return new WaitForSeconds(20f);
        seedsReady = true;
        StopCoroutine("SeedSpawnTimer");
    }
}
