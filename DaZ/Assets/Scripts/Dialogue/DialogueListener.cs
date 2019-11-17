using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueListener : MonoBehaviour
{
    public bool Dialogueenabled = false;
    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && Dialogueenabled)
            Next();
    }

    public void Next()
    {
        FindObjectOfType<DialogueBehavior>().DisplayNextSentence();
    }
    
}
