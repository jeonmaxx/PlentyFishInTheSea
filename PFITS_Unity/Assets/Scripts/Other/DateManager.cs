using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
    public DialogueSo dateDialogue;
    public GameObject eventPicture;
    private DialogueManager dialogueManager;
    private DayManager dayManager;
    public GameObject Ui;

    [HideInInspector] public Message[] messages;
    [HideInInspector] public Actor[] actors;
    [HideInInspector] public List<AnswerSo> answers;
    public GameObject dateRoom;

    public bool dateInProgress;
    public bool endScreen;
    public bool dateOver;

    private void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        dayManager = FindAnyObjectByType<DayManager>();
    }

    private void Update()
    {
        if (!dialogueManager.isActive && dateInProgress && !endScreen)
        {       
            StartCoroutine(PopUpCoroutine(true, 1f));
        }
    }

    private IEnumerator StartDate()
    {
        yield return new WaitForSeconds(1);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        dateInProgress = true;
    }

    private void GetDialogue()
    {
        messages = dateDialogue.messages.ToArray();
        actors = new Actor[dateDialogue.characters.Length];
        for (int k = 0; k < dateDialogue.characters.Length; k++)
        {
            actors[k] = dateDialogue.characters[k].actor;
        }
        answers = new List<AnswerSo>(dateDialogue.answers);
    }

    public void StartDialogue()
    {
        GetDialogue();
        dialogueManager.OpenDialogue(messages, actors, answers, dateDialogue);
        StartCoroutine(StartDate());
    }

    private IEnumerator PopUpCoroutine(bool open, float openTime)
    {
        dayManager.StartCoroutine(dayManager.FadeToIntensity(1, dayManager.fadeDuration));
        Image image = eventPicture.GetComponentInChildren<Image>();
        if (open)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
            eventPicture.transform.localScale = Vector3.one;

            float elapsedTime = 0f;

            while (elapsedTime < openTime)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / openTime);
                image.color = color;
                yield return null;
            }
            color.a = 1f;
            image.color = color;
            endScreen = true;
        }
        else
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;

            float elapsedTime = 0f;

            while (elapsedTime < openTime)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(1f - (elapsedTime / openTime));
                image.color = color;
                yield return null;
            }

            color.a = 0f;
            image.color = color;
            eventPicture.transform.localScale = Vector3.zero;
        }
    }

    public void CloseButton()
    {
        StartCoroutine(PopUpCoroutine(false, 1f));
        dateInProgress = false;
    }
}
