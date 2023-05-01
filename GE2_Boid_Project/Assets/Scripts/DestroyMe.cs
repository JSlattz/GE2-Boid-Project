using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public bool bigExplosion = false;

    void Start()
    {
        if(bigExplosion)
        {
            float rnd = Random.Range(2f, 10f);
            transform.localScale = new Vector3(rnd, rnd, rnd);
        }

        Destroy(gameObject, 2.5f);
    }
}
