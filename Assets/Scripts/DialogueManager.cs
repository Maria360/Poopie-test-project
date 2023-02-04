using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    private Queue<string> sentences;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI displayText;

    private string activeSentence;
    public float typingSpeed;

    private AudioSource myAudio;
    //[SerializeField] AudioClip speakSound;

    private void Start()
    {
        sentences = new Queue<string>();
        myAudio = GetComponent<AudioSource>();
    }

    void StartDialogue()
    {//borra el queu las añade desde el principio
        sentences.Clear();

        foreach(string sentence in dialogue.sentenceList)
        {
            sentences.Enqueue(sentence); //llama la oracion y la añade
        }

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (sentences.Count <= 0)
        {
            displayText.text = activeSentence;
            return;
        }

        activeSentence = sentences.Dequeue();
        displayText.text = activeSentence;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(activeSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {//animacion del texto
        displayText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            displayText.text += letter;
            //myAudio.PlayOneShot(speakSound);
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
            StartDialogue();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Return) && displayText.text == activeSentence)
            {
                DisplayNextSentence();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
            StopAllCoroutines();
        }
    }
}
