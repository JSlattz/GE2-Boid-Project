using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class Seek : SteeringBehaviour
{
    public GameObject targetGameObject;
    public Vector3 target;
    public GameObject[] foundFood;
    SteeringBehaviour steeringBehaviour;
    GameObject go;
    GameObject closest;
    //public LayerMask Food;

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            if (targetGameObject != null)
            {
                target = closest.transform.position;
            }
            Gizmos.DrawLine(transform.position, target);
        }
    }
    
    public override Vector3 Calculate()
    {
        return boid.SeekForce(target);    
    }

    private void Start()
    {

    }

    public void Update()
    {
        if (targetGameObject != null)
        {
            target = closest.transform.position;
        }
        LookForFood();
    }

    public GameObject LookForFood()
    {
        foundFood = GameObject.FindGameObjectsWithTag("Food");
        closest = null;

        Vector3 pos = transform.position;

        float distance = Mathf.Infinity;

        foreach (GameObject go in foundFood)
        {
            Vector3 diff = go.transform.position - pos;

            float currDistance = diff.sqrMagnitude;

            if (currDistance < distance)
            {
                closest = go;
                distance = currDistance;
            }
        }
        targetGameObject = closest;
        return closest;
    }

}