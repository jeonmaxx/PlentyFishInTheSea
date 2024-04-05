using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;

    [HideInInspector] public Message[] currentMessages;
    private Actor[] currentActors;
    [HideInInspector] public int activeMessage = 0;
    public bool isActive = false;

    public InputActionReference inputAction;
    private InputAction action;

    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource source;

    public void Start()
    {
        action = inputAction.action;
        //source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        action.started += _ => OnSkip();
    }

    public void OnSkip()
    {
        if (isActive == true)
        {
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        if (!isActive)
        {
            //source.clip = openSound;
            //source.Play();
            currentMessages = messages;
            currentActors = actors;
            activeMessage = 0;                    
            DisplayMessage();
            StartCoroutine(StartDialogue());
        }
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

    }

    public void NextMessage()
    {
        if (isActive)
        {
            activeMessage++;
            if (activeMessage < currentMessages.Length)
            {
                DisplayMessage();
            }
            else
            {
                //source.clip = closeSound;
                //source.Play();
                StartCoroutine(EndDialogue());
                Debug.Log("dialogue ended");
            }
        }
    }
   

    private IEnumerator EndDialogue()
    {
        yield return new WaitForEndOfFrame();
        transform.transform.localScale = Vector3.zero;
        isActive = false;
        StopCoroutine(EndDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForEndOfFrame();
        isActive = true;
        StopCoroutine(StartDialogue());
    }

    public void LeaveButton()
    {
        transform.localScale = Vector3.zero;
    }
}
