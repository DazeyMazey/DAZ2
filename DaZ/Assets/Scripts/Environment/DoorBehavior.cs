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

    // This function makes the player walk back a couple of paces and brings up the dialogue box from Bud
    // saying that he's going to fix DAZ no matter what and will need as many pieces as he can find
    public void WalkPlayerBack()
    { 
        
    }
}
