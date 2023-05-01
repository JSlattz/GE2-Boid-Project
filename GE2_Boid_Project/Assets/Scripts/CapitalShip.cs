using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : MonoBehaviour
{
    public enum Faction { Rebel, NewEast};
    public Faction faction;

    public Vector3 centre, radius;

    public GameObject deathAnimation;
    public Camera cam;
    public ParticleSystem explosion;

    public CameraSwitch camSwitch;
    public ReloadScene reloadScene;

    bool active;
    GameObject[] agents;

    private void Awake()
    {
        cam.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (faction)
        {
            case Faction.Rebel:
                agents = GameObject.FindGameObjectsWithTag("R");
                break;

            case Faction.NewEast:
                agents = GameObject.FindGameObjectsWithTag("NE");
                break;
        }

        if(agents.Length <= 0 && !active && Time.timeSinceLevelLoad >= 25f)
        {
            Death();
            active = true;
        }
    }

    void Death()
    {
        reloadScene.end = true;
        camSwitch.enabled = false;
        cam.gameObject.SetActive(true);
        deathAnimation.gameObject.SetActive(true);
        InvokeRepeating("Explosion", 0, 0.01f);
        Destroy(gameObject, 5f);
    }

    void Explosion()
    {
        float x = Random.Range(centre.x - radius.x / 2, centre.x + radius.x / 2);
        float y = Random.Range(centre.y - radius.y / 2, centre.y + radius.y / 2);
        float z = Random.Range(centre.z - radius.z / 2, centre.z + radius.z / 2);

        Vector3 pos = new Vector3(x, y, z);
        Instantiate(explosion, pos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(centre, radius);
    }
}
