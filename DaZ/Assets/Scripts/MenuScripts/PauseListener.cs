using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseListener : MonoBehaviour
{
    GameObject[] pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseMenu = GameObject.FindGameObjectsWithTag("PauseMenu");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    private void showPaused()
    {
        foreach (GameObject g in pauseMenu)
            g.SetActive(true);
    }

    private void hidePaused()
    {
        foreach (GameObject g in pauseMenu)
            g.SetActive(false);
    }


    public void Resume()
    {
        Time.timeScale = 1;
        hidePaused();
    }

}
