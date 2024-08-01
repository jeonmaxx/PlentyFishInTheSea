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
    public bool useSpecialSound;
    public AudioClip[] specialSound;
    private AudioClip nOpenSound;
    private AudioClip nCloseSound;
    public List<AnswerSo> answers;



    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        //nOpenSound = dialogue.openSound;
        //nCloseSound = dialogue.closeSound;   
    }

    private void Update()
    {
        ChooseDialogue();
    }

    private void ChooseDialogue()
    {
        DialogueSo selectedDialogue = null;
        List<DialogueSo> prioritizedDialogues = new List<DialogueSo>();

        for (int i = 0; i < dialogueSo.Count; i++)
        {
            if (dayManager.dayList.days[dayManager.currentDayInt] == dialogueSo[i].day)
            {
                bool affinityMatched = false;
                for (int j = 0; j < dialogueSo[i].characters.Length; j++)
                {
                    if (dialogueSo[i].affinity.Contains(dialogueSo[i].characters[j].affinity))
                    {
                        affinityMatched = true;
                        break;
                    }
                }

                if (affinityMatched)
                {
                    if (dialogueSo[i].firstTimeNpc && !dialogueSo[i].knownNpc)
                    {
                        selectedDialogue = dialogueSo[i];
                        break;
                    }
                    else if (dialogueSo[i].neededClue != null && dialogue.clueManager.clues.Contains(dialogueSo[i].neededClue))
                    {
                        prioritizedDialogues.Add(dialogueSo[i]);
                    }
                    else if (dialogueSo[i].neededClue == null)
                    {
                        selectedDialogue = dialogueSo[i];
                    }
                }
            }
        }

        if (selectedDialogue == null && prioritizedDialogues.Count > 0)
        {
            selectedDialogue = prioritizedDialogues[0];
        }

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
        //if (useSpecialSound)
        //{
        //    dialogue.openSound = specialSound[0];
        //    dialogue.closeSound = specialSound[1];
        //}
        //else
        //{
        //    dialogue.openSound = nOpenSound;
        //    dialogue.closeSound = nCloseSound;
        //}

        dialogue.OpenDialogue(messages, actors, answers);
        dialogue.currentNpc = GetComponent<DialogueTrigger>();
    }

    public void SetKnownNpc()
    {
        if (currentDialogue != null && currentDialogue.firstTimeNpc)
        {
            currentDialogue.knownNpc = true;
        }
    }
}