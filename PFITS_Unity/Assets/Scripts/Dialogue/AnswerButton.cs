using Unity.VisualScripting;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public AnswerSo answer;
    public DialogueManager manager;
    private Actor[] nextActors;
    public DialogueSo currentDialogue;
    private NpcManager npcManager;

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
    }

    public void AnswerClick()
    {
        nextActors = new Actor[answer.nextDialogue.characters.Length];
        for (int i = 0; i < answer.nextDialogue.characters.Length; i++)
        {
            nextActors[i] = answer.nextDialogue.characters[i].actor;
        }
        answer.clicked = true;
        manager.AnswerButton(answer.nextDialogue.messages.ToArray(), nextActors, answer.nextDialogue.answers);
        //manager.currentNpc.currentAffinity += answer.addedAffinity;
        answer.nextDialogue.characters[0].affinity += answer.addedAffinity;

        if (answer.isChore != null && answer.questAnswer)
        {
            SetChoreAsDone(answer);
        }
        else if(answer.isChore == null && answer.questAnswer)
        {
            Debug.LogWarning("No chore found!");
        }
    }

    private void SetChoreAsDone(AnswerSo answer)
    {
        foreach (var npc in npcManager.npcObjects)
        {
            if (npc.character == answer.isChore.interviewNpc)
            {
                RemoveAnswerFromDialogue(npc.characterObj, answer);
                answer.isChore.done = true;
                break;
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
