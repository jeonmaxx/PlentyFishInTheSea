using System.Collections;
using UnityEngine;
public enum Emotions { Neutral, Happy, Angry, Sad}

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public Message[] messages;
    [HideInInspector] public Actor[] actors;
    public DialogueSo[] dialogueSo;
    private DialogueManager dialogue;
    public bool useSpecialSound;
    public AudioClip[] specialSound;
    private AudioClip nOpenSound;
    private AudioClip nCloseSound;
    //public int currentAffinity;
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
            // if (dialogueSo[i].affinity == currentAffinity)
            if (dialogueSo[i].affinity == dialogueSo[i].characters[0].affinity)
            {
                messages = dialogueSo[i].messages;
                for (int j = 0; j < dialogueSo[j].characters.Length; j++)
                {
                    actors[j] = dialogueSo[i].characters[j].actor;
                }
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
        dialogue.currentNpc = GetComponent<DialogueTrigger>();
    }
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
    public Emotions emotion;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
    public Sprite happySprite;
    public Sprite angrySprite;
}

[System.Serializable]
public class Answer
{
    public string answerText;
    public DialogueSo nextDialogue;
    public int addedAffinity;
}
