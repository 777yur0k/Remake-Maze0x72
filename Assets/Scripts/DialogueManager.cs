using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI dialogText;
    Queue<string> sentences;

    void Start() => sentences = new Queue<string>();

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        foreach(string sentence in dialogue.sentences) sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
    
        foreach(char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    public void CloseDialog() => EndDialogue();

    void EndDialogue()
    {
        StopAllCoroutines();
        animator.SetBool("IsOpen", false);
    }
}