using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public DialogueSo[] dialogueSo;
    private DialogueManager dialogue;
    public bool useSpecialSound;
    public AudioClip[] specialSound;
    private AudioClip nOpenSound;
    private AudioClip nCloseSound;
    public int currentAffinity;
    public Answer[] answers;
 

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        nOpenSound = dialogue.openSound;
        nCloseSound = dialogue.closeSound;   
    }

    private void Update()
    {
        for (int i = 0; i < dialogueSo.Length; i++)
        {
            if (dialogueSo[i].affinity == currentAffinity)
            {
                messages = dialogueSo[i].messages;
                actors = dialogueSo[i].actors;
                answers = dialogueSo[i].answers;
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
    }
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}

[System.Serializable]
public class Answer
{
    public string answerText;
    public DialogueSo nextDialogue;
}
