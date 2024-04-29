using Unity.VisualScripting;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public Answer answer;
    public DialogueManager manager;
    private Actor[] nextActors;

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
        manager.AnswerButton(answer.nextDialogue.messages, nextActors, answer.nextDialogue.answers);
        //manager.currentNpc.currentAffinity += answer.addedAffinity;
        answer.nextDialogue.characters[0].affinity += answer.addedAffinity;
    }
}
