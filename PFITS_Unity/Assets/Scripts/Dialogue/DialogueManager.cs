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
    public int currentDay;
    public TextMeshProUGUI dayText;

    [HideInInspector] public Message[] currentMessages;
    [HideInInspector] public Actor[] currentActors;
    [HideInInspector] public int activeMessage = 0;
    public bool isActive = false;
    [HideInInspector] public Answer[] currentAnswers;
    public DialogueTrigger currentNpc;

    public InputActionReference inputAction;
    private InputAction action;

    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource source;

    public GameObject buttonSpawner;
    public GameObject buttonPrefab;

    private bool inAnswerScreen = false;

    public void Start()
    {
        action = inputAction.action;
        //source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        action.started += _ => OnSkip();
        dayText.text = "Day: " + currentDay;
    }

    public void OnSkip()
    {
        if (isActive == true)
        {
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, Answer[] answers)
    {
        if (!isActive)
        {
            //source.clip = openSound;
            //source.Play();
            currentMessages = messages;
            currentActors = actors;
            currentAnswers = answers;
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
        if (messageToDisplay.emotion == Emotions.Neutral)
        {
            actorImage.sprite = actorToDisplay.sprite;
        }
        else if(messageToDisplay.emotion == Emotions.Angry)
        {
            actorImage.sprite = actorToDisplay.angrySprite;
        }
        else if(messageToDisplay.emotion == Emotions.Happy)
        {
            actorImage.sprite = actorToDisplay.happySprite;
        }
    }

    public void NextMessage()
    {
        if (isActive && !inAnswerScreen)
        {
            activeMessage++;
            if (activeMessage < currentMessages.Length)
            {
                DisplayMessage();
                if(activeMessage == currentMessages.Length-1 && currentAnswers.Length > 0)
                {
                    MakeAnswerButtons();
                    inAnswerScreen = true;
                }
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

    public void MakeAnswerButtons()
    {
        for(int i = 0; i < currentAnswers.Length; i++)
        {
            GameObject currentButton = Instantiate(buttonPrefab, buttonSpawner.transform);
            currentButton.GetComponentInChildren<TextMeshProUGUI>().text = currentAnswers[i].answerText;
            currentButton.GetComponent<AnswerButton>().answer = currentAnswers[i];
        }
    }

    public void AnswerButton(Message[] messages, Actor[] actors, Answer[] answers)
    {
        inAnswerScreen = false;
        currentMessages = messages;
        currentActors = actors;
        currentAnswers = answers;
        activeMessage = 0;
        foreach (Transform child in buttonSpawner.transform)
        {
            Destroy(child.gameObject);
        }
        DisplayMessage();
        StartCoroutine(StartDialogue());
    }
   

    private IEnumerator EndDialogue()
    {
        yield return new WaitForEndOfFrame();
        transform.localScale = Vector3.zero;
        isActive = false;
        foreach(Transform child in buttonSpawner.transform)
        {
            Destroy(child.gameObject);
        }
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
