using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorBehavior : MonoBehaviour
{
    public int doorIndex;
    public int requiredNumber;

    public bool player_enter_from_left;

    private _Dialogue[] go_back;

    public void Start()
    {
        go_back = new _Dialogue[2];
        go_back[0] = new _Dialogue();
        go_back[0].name = "D.A.-Z";
        go_back[0].sentences = new string[]{ 
            "Hey Bud, I don't think you've collected everything on this floor", 
            "I'd appreciate it if you collected as much as you can", 
            "I'm counting on you!"
        };
        go_back[1] = new _Dialogue();
        go_back[1].name = "Bud";
        go_back[1].sentences = new string[]{
            "Don't worry D.A.-Z, I'll put you back together."
        };
    }

    public void NextLevel(int totalItemsCollected)
    {
        if (totalItemsCollected >= requiredNumber)
            FindObjectOfType<Fades>().FadeOut();
        else
            WalkPlayerBack();
    }

    // This function makes the player walk back a couple of paces and brings up the dialogue box from Bud
    // saying that he's going to fix DAZ no matter what and will need as many pieces as he can find
    public void WalkPlayerBack()
    {
        PlayerInput Player = FindObjectOfType<PlayerInput>();
        Player.PlayerEnabled = false;

        if (player_enter_from_left)
            Player.PlayerCutSceneLeft = true;
        else
            Player.PlayerCutSceneRight = true;

        FindObjectOfType<DialogueBehavior>().StartDialogue(go_back, 0);
        FindObjectOfType<DialogueListener>().Dialogueenabled = true;

        Invoke("PlayerStop", 0.3f);

    }

    private void PlayerStop()
    {
        PlayerInput Player = FindObjectOfType<PlayerInput>();
        Player.PlayerCutSceneRight = false;
        Player.PlayerCutSceneLeft = false;
    }


    public void GoNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
