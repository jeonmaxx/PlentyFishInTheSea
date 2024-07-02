using UnityEngine;
using System;

[CreateAssetMenu]
public class AnswerSo : ScriptableObject
{
    public string answerText;
    public DialogueSo nextDialogue;
    public int addedAffinity;
    public bool clicked;
    public bool questAnswer;
    [Tooltip ("Only if its an chore answer!")]
    public ChoreSo isChore;
    public string id;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }
    }
}

