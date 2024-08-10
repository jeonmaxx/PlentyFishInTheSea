using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueRoom : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public Message[] messages;
    [HideInInspector] public Actor[] actors;
    public DayManager dayManager;
    public List<DialogueSo> dialogueSo;
    public DialogueSo currentDialogue;
    private DialogueManager dialogue;
    public List<AnswerSo> answers;
    private ClueManager clueManager;
    private MouseOnDoor mouseOnDoor;
    private bool hasValidDialogue = false;
    private RoomManager roomManager;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        clueManager = FindObjectOfType<ClueManager>(); 
        mouseOnDoor = GetComponent<MouseOnDoor>();
        roomManager = FindObjectOfType<RoomManager>();
    }

    private void Update()
    {
        foreach (DialogueSo dialogue in dialogueSo)
        {
            if (roomManager.activeRoom.GetComponent<RoomDefinition>().room == dialogue.room && !dialogue.knownRoom && dialogue.roomTalk)
            {
                Debug.Log("fitting Dialogue found");
                StartDialogue(false);
            }
        }
    }

    private void GetDialogue()
    {
        DialogueSo selectedDialogue = null;
        selectedDialogue = GetRightDialogue();

        if (selectedDialogue != null)
        {
            currentDialogue = selectedDialogue;
            messages = currentDialogue.messages.ToArray();
            actors = new Actor[currentDialogue.characters.Length];
            for (int k = 0; k < currentDialogue.characters.Length; k++)
            {
                actors[k] = currentDialogue.characters[k].actor;
            }
            answers = new List<AnswerSo>(currentDialogue.answers);
            hasValidDialogue = true;
        }
        else
        {
            hasValidDialogue = false;
        }
    }

    public void StartDialogue(bool closeDoor)
    {
        GetDialogue();
        if (hasValidDialogue)
        {
            if (closeDoor)
            {
                mouseOnDoor.cantLeave = true;
            }
            dialogue.OpenDialogue(messages, actors, answers, currentDialogue);
            dialogue.dialogueRoom = GetComponent<DialogueRoom>();
            currentDialogue.knownRoom = true;
        }
        else
        {
            if (closeDoor)
            {
                mouseOnDoor.cantLeave = false;
                mouseOnDoor.Leaving();
            }
        }
    }

    private DialogueSo GetRightDialogue()
    {
        foreach (DialogueSo dialogue in dialogueSo)
        {
            if (dayManager.dayList.days[dayManager.currentDayInt] == dialogue.day && dialogue.roomTalk)
            {
                if ((dialogue.entersRoom && !dialogue.knownRoom) || (!dialogue.entersRoom && !clueManager.foundClues.Contains(dialogue.clueToLeave)))
                {
                    return dialogue;
                }
            }
        }
        return null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDialogue(true);
    }
}
