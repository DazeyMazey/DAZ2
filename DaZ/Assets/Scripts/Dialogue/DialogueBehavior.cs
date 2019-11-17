using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueBehavior : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

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
            FindObjectOfType<PlayerInput>().PlayPlayer();
            FindObjectOfType<DialogueListener>().Dialogueenabled = false;
        }
    }
}
