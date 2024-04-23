using Unity.VisualScripting;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public Answer answer;
    public DialogueManager manager;

    private void Update()
    {
        if(manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>(); 
        }
    }

    public void AnswerClick()
    {
        manager.AnswerButton(answer.nextDialogue.messages, answer.nextDialogue.actors, answer.nextDialogue.answers);
    }
}
