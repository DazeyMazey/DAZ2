﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueBehavior : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    //dialogue box
    public Animator animator;
    //character to anim
    public Animator animator2;

    private Queue<string> sentences; // list of words
    private _Dialogue[] dialogues;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(_Dialogue[] dialogue, int index)
    {
        // for clearing the E's on screen during dialogue
        E_Behavior[] Interactables = FindObjectsOfType<E_Behavior>();
        foreach (E_Behavior e in Interactables)
            e.InDialogue = true;


        animator.SetBool("is_open", true);
        dialogues = dialogue;
        i = index;


        sentences.Clear();
        nameText.text = dialogue[i].name;

        foreach (string item in dialogue[i].sentences)
        {
            sentences.Enqueue(item);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
           
            EndDialogue();
            return;
        }
        if (animator2)
            animator2.SetBool("istalk", true);
        
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
     
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

    }


    private void EndDialogue()
    {
        animator.SetBool("is_open", false);
        
        if (i < dialogues.Length -1)
            StartDialogue(dialogues, ++i);
        else
        {
            if (animator2)
                animator2.SetBool("istalk", false);

            FindObjectOfType<PlayerInput>().PlayPlayer();
            FindObjectOfType<DialogueListener>().Dialogueenabled = false;
            E_Behavior[] Interactables = FindObjectsOfType<E_Behavior>();
            foreach (E_Behavior e in Interactables)
                e.InDialogue = false;
        }
    }
}
