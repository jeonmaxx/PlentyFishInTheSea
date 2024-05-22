using UnityEngine;
using System.Collections.Generic;

public enum Emotions { Neutral, Happy, Angry, Sad}
public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public Message[] messages;
    [HideInInspector] public Actor[] actors;
    public List<DialogueSo> dialogueSo;
    public DialogueSo currentDialogue;
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
        //nOpenSound = dialogue.openSound;
        //nCloseSound = dialogue.closeSound;   
    }

    private void Update()
    {
        for (int i = 0; i < dialogueSo.Count; i++)
        {
            // if (dialogueSo[i].affinity == currentAffinity)
            if (dialogueSo[i].affinity == dialogueSo[i].characters[0].affinity)
            {
                currentDialogue = dialogueSo[i];
                messages = currentDialogue.messages.ToArray();
                for (int j = 0; j < currentDialogue.characters.Length; j++)
                {
                    actors[j] = currentDialogue.characters[j].actor;
                }
                answers = currentDialogue.answers.ToArray();
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