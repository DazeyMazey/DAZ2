using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public AudioSource Door;

    public void DoorBell()
        {
            Door.Play();
        }

}
