using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentalHazardBehaivor_Timer : MonoBehaviour
{
    public int hazard_time;
    public bool is_on;

    // Start is called before the first frame update
    void Start()
    {
        Turn_On();
    }

    private void Turn_On()
    {
        is_on = true;
        this.gameObject.tag = "Hazard";
        this.gameObject.SetActive(true);
        Invoke("Turn_Off", hazard_time);
    }

    private void Turn_Off()
    {
        is_on = false;
        this.gameObject.tag = "Untagged";
        this.gameObject.SetActive(false);

        Invoke("Turn_On", hazard_time);
    }
}
