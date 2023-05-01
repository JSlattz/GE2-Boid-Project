using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public float startTimer = 1f;
    float timer;
    int rnd;

    public List<Camera> camsList = new List<Camera>();

    Camera[] camsArray;
    Camera current, cam;

    void Start()
    {
        timer = 0;
        ChangeCamera();
    }

    void Update()
    {
        camsArray = camsList.ToArray();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChangeCamera();
        }

        if (cam = null)
            ChangeCamera();
    }

    public void ChangeCamera()
    {
        rnd = Random.Range(0, camsArray.Length);

        if (current != null)
            current.gameObject.SetActive(false);

        camsArray[rnd].gameObject.SetActive(true);

        timer = startTimer;
    }
}
