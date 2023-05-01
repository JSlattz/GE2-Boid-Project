using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nematode : MonoBehaviour
{
    public int lengthL;
    private int length;
    public int lengthMin = 5;
    public int lengthMax = 25;
    int j = 0;

    public GameObject firstSphere;

    public Material material;

    void Awake()
    {


        length = Random.Range(lengthMin, lengthMax);
        lengthL = length;

        for(int i = 0; i < length; i++ )
        {
            float height = 1f / length * i;

             firstSphere = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshRenderer meshRenderer = firstSphere.GetComponent<MeshRenderer>();
            if (i == 0)
            {
                firstSphere.AddComponent<Boid>();
                firstSphere.AddComponent<ObstacleAvoidance>();
                firstSphere.AddComponent<NoiseWander>();
                firstSphere.AddComponent<Constrain>();

            }

            firstSphere.transform.position = new Vector3(0f, 0f, i);
            meshRenderer.material.color = Color.HSVToRGB(height, 1f, 1f);
            firstSphere.transform.SetParent(transform);
            firstSphere.transform.localPosition = new Vector3(0f, 0f, i * 1f);

        }
        if (j <= Mathf.Ceil(length / 2))
        {
            firstSphere.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            firstSphere.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
