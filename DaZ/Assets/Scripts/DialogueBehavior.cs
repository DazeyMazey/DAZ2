using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueBehavior : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

   // public Animator animator;

    private Queue<string> sentences; // list of words

    private _Dialogue[] dialogues;
    private int i;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(_Dialogue dialogue)
    {
     //   animator.SetBool("IsOpen", true);

        sentences.Clear();
        nameText.text = dialogue.names[0];

        foreach (string item in dialogue.sentences)
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
        Debug.Log("end of conversation");
      //  animator.SetBool("IsOpen", false);
        FindObjectOfType<PlayerInput>().PlayPlayer();
        FindObjectOfType<DialogueListener>().Dialogueenabled = false;
    }
}
