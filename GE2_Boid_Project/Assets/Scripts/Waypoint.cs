using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    float x, y, z;

    void Start()
    {
        x = Random.Range(-50f, 50f);
        y = Random.Range(-50f, 50f);
        z = Random.Range(-50f, 50f);

        transform.localPosition = new Vector3(x, y, z);
    }

    void Update()
    {
        
    }
}
