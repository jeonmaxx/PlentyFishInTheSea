using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum Emotions { Neutral, Happy, Angry, Sad, Disgust, Fear, Surprise, Evil, Excited }
public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public Message[] messages;
    [HideInInspector] public Actor[] actors;
    public DayManager dayManager;
    public List<DialogueSo> dialogueSo;
    public DialogueSo currentDialogue;
    private DialogueManager dialogue;
    public List<AnswerSo> answers;
    public List<DialogueSo> priorityList;


    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        priorityList = GetPrioritizedDialogues();  
    }

    private void Update()
    {
        priorityList = GetPrioritizedDialogues();
    }

    private void GetDialogue()
    {
        DialogueSo selectedDialogue = null;
        selectedDialogue = priorityList[0];

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
        }
    }

    public void StartDialogue()
    {
        GetDialogue();
        dialogue.OpenDialogue(messages, actors, answers, currentDialogue);
        dialogue.currentNpc = GetComponent<DialogueTrigger>();
    }

    public void SetKnownNpc()
    {
        if (currentDialogue != null && currentDialogue.firstTimeNpc)
        {
            currentDialogue.knownNpc = true;
        }
        if (currentDialogue.oneTimeDia)
        {
            currentDialogue.diaDone = true;
        }
    }

    private List<DialogueSo> GetPrioritizedDialogues()
    {
        List<PriorityDialogue> prioritizedDialogues = new List<PriorityDialogue>();

        foreach (var current in dialogueSo)
        {
            if (current.oneTimeDia && current.diaDone)
            {
                continue;
            }

            if (dayManager.dayList.days[dayManager.currentDayInt] != current.day)
            {
                continue;
            }

            bool affinityMatched = current.characters.Any(character => current.affinity.Contains(character.affinity));
            if (!affinityMatched)
            {
                continue;
            }

            if (current.firstTimeNpc && current.knownNpc)
            {
                continue;
            }

            int priorityLevel = int.MaxValue;

            if (current.firstTimeNpc && !current.knownNpc)
            {
                if (current.neededClue == null && current.neededChore == null)
                {
                    priorityLevel = 1;
                }
                else if (current.neededClue != null)
                {
                    bool clueInList = dialogue.clueManager.foundClues.Contains(current.neededClue);
                    if (current.dialogueIfNoClue && !clueInList)
                    {
                        priorityLevel = 2;
                    }
                    else if (!current.dialogueIfNoClue && clueInList)
                    {
                        continue;
                    }
                }
                else if (current.neededChore != null)
                {
                    bool choreInList = dayManager.chores.Contains(current.neededChore);
                    if (current.dialogueIfNotThere && !choreInList)
                    {
                        priorityLevel = 4;
                    }
                    else if (!current.dialogueIfNotThere && choreInList)
                    {
                        continue;
                    }
                }
            }
            else
            {
                if (current.neededClue != null && !current.firstTimeNpc)
                {
                    bool clueInList = dialogue.clueManager.foundClues.Contains(current.neededClue);
                    if ((current.dialogueIfNoClue && !clueInList) || (!current.dialogueIfNoClue && clueInList))
                    {
                        priorityLevel = 6;
                    }
                    else if ((current.dialogueIfNoClue && clueInList) ||(!current.dialogueIfNoClue && !clueInList))
                    {
                        continue;
                    }
                }
                else if (current.neededChore != null && !current.firstTimeNpc)
                {
                    bool choreInList = dayManager.chores.Contains(current.neededChore);
                    if ((current.dialogueIfNotThere && !choreInList) || (!current.dialogueIfNotThere && choreInList))
                    {
                        priorityLevel = 8;
                    }
                    else if ((current.dialogueIfNotThere && choreInList) || (!current.dialogueIfNotThere && !choreInList))
                    {
                        continue;
                    }
                }
                else
                {
                    priorityLevel = 10;
                }
            }

            prioritizedDialogues.Add(new PriorityDialogue(current, priorityLevel));
        }

        var sortedDialogues = prioritizedDialogues.OrderBy(p => p.PriorityLevel)
                                                  .ThenByDescending(p => p.Dialogue.oneTimeDia)
                                                  .Select(p => p.Dialogue)
                                                  .ToList();

        return sortedDialogues;
    }
}