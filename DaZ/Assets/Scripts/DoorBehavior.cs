using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehavior : MonoBehaviour
{
    public int doorIndex;
    public int requiredNumber;


    public void NextLevel(int totalItemsCollected)
    {
        if (totalItemsCollected >= requiredNumber)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       
            // we could have a text box or something of the like pop up
    }
}
