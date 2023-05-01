using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NematodeSchool : MonoBehaviour
{
    public GameObject prefab;

    [Range (1, 5000)]
    public int radius = 50;
    
    public int count = 10;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < count; i++)
        {
            float rnd = Random.Range(0f, 360f);
            Vector3 pos = Random.insideUnitSphere * radius;
            Quaternion rotation = Quaternion.Euler(0f, rnd, 0f);
            Instantiate(prefab, pos, rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
