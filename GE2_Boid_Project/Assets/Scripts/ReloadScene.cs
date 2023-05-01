using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReloadScene : MonoBehaviour
{
    [HideInInspector]
    public bool end;

    public TextMeshProUGUI text;

    private void Awake()
    {
        GetComponent<CameraSwitch>().enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(Time.timeSinceLevelLoad >= 25f && !end)
        {
            GetComponent<CameraSwitch>().enabled = true;
        }

        ChangeTimeScale();

        if(end)
        {
            Invoke("ShowReload", 6.5f);
        }
    }

    void ShowReload()
    {
        text.gameObject.SetActive(true);
        GetComponent<CameraSwitch>().enabled = true;
    }

    void ChangeTimeScale()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale++;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale--;
        }
    }
}
