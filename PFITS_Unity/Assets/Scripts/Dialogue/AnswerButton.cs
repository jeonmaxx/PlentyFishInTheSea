using Unity.VisualScripting;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public Answer answer;
    public DialogueManager manager;
    private Actor[] nextActors;
    public DialogueSo currentDialogue;

    private void Update()
    {
        if(manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>(); 
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
        manager.AnswerButton(answer.nextDialogue.messages.ToArray(), nextActors, answer.nextDialogue.answers.ToArray());
        //manager.currentNpc.currentAffinity += answer.addedAffinity;
        answer.nextDialogue.characters[0].affinity += answer.addedAffinity;

        if (answer.questAnswer && answer.isChore != null)
        {
            answer.isChore.done = true;
            for (int i = 0; i < currentDialogue.answers.Count; i++)
            {
                if (currentDialogue.answers[i] == answer)
                {
                    currentDialogue.answers.Remove(answer);
                }
            }
        }
        else if(answer.questAnswer && answer.isChore == null)
            Debug.LogWarning("No chore found!");
    }
}
