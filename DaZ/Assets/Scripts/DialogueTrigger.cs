﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public _Dialogue[] dialogue;

    public void interact()
    {
        FindObjectOfType<DialogueBehavior>().StartDialogue(dialogue, 0);
        FindObjectOfType<DialogueListener>().Dialogueenabled = true;
    }
}
