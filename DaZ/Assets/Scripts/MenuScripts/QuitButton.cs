﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit the Game with Exit Code: 0");
        Application.Quit();
    }
}
