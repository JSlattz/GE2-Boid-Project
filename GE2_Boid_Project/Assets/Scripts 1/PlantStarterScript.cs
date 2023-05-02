using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStarterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<PlantScript>();
        gameObject.GetComponent<PlantScript>().plantStems.Clear();
        gameObject.GetComponent<PlantStarterScript>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
