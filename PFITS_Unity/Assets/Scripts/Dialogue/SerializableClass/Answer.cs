using UnityEngine;

[System.Serializable]
public class Answer
{
    public string answerText;
    public DialogueSo nextDialogue;
    public int addedAffinity;
    public bool clicked;
    public bool questAnswer;
    [Tooltip ("Only if its an chore answer!")]
    public ChoreSo isChore;
}

