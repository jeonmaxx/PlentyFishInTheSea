using Unity.VisualScripting;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public AnswerSo answer;
    public DialogueManager manager;
    private Actor[] nextActors;
    public DialogueSo currentDialogue;
    private NpcManager npcManager;
    private DayManager dayManager;

    private void Update()
    {
        if(manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>(); 
        }

        if (npcManager == null)
        {
            npcManager = FindObjectOfType<NpcManager>();
        }

        if(dayManager == null)
        {
            dayManager = FindObjectOfType<DayManager>();
        }
    }

    public void AnswerClick()
    {
        manager.currentDialogue = answer.nextDialogue;
        Debug.Log("current Dialogue should be changed");
        nextActors = new Actor[answer.nextDialogue.characters.Length];
        for (int i = 0; i < answer.nextDialogue.characters.Length; i++)
        {
            nextActors[i] = answer.nextDialogue.characters[i].actor;
        }
        answer.clicked = true;
        manager.AnswerButton(answer.nextDialogue.messages.ToArray(), nextActors, answer.nextDialogue.answers);

        answer.nextDialogue.characters[0].affinity += answer.addedAffinity;

        if (answer.isChore != null)
        {
            SetChoreAsDone(answer);
        }
    }

    private void SetChoreAsDone(AnswerSo answer)
    {
        if(answer.isChore != null && answer.isChore.noted)
        {
            if(answer.isChore.type == ChoreType.InterviewOne)
            {
                answer.isChore.done = true;
            }

            if(answer.isChore.type == ChoreType.InterviewNumber)
            {
                answer.isChore.currentInterviewed++;
                if(answer.isChore.currentInterviewed >= answer.isChore.npcsToInterview)
                {
                    answer.isChore.done = true;
                }
                dayManager.AddChores();
            }
        }
    }

    private void RemoveAnswerFromDialogue(GameObject characterObj, AnswerSo answer)
    {
        var dialogueTrigger = characterObj.GetComponent<DialogueTrigger>();
        foreach (var dialogue in dialogueTrigger.dialogueSo)
        {
            dialogue.answers.RemoveAll(a => a.isChore == answer.isChore);
        }
    }
}
