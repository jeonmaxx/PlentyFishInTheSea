using UnityEngine;
using System.Collections.Generic;

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
        for (int i = 0; i < dialogueSo.Count; i++)
        {
            if (dialogueSo[i].affinity == dialogueSo[i].characters[0].affinity && dayManager.dayList.days[dayManager.currentDayInt] == dialogueSo[i].day)
            {
                currentDialogue = dialogueSo[i];
                messages = currentDialogue.messages.ToArray();
                actors = new Actor[currentDialogue.characters.Length];
                for (int j = 0; j < currentDialogue.characters.Length; j++)
                {
                    actors[j] = currentDialogue.characters[j].actor;
                }
                answers = new List<AnswerSo>(currentDialogue.answers);
            }
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
}