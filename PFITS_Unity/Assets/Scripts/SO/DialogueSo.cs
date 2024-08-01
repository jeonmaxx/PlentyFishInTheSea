using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSo : ScriptableObject
{
    public int[] affinity;
    public int day;

    public CharacterSo[] characters;
    public List<Message> messages;
    public List<AnswerSo> answers;

    public bool selfTalk;
    public bool roomTalk;
    public Rooms room;
    public bool knownRoom;

    public bool firstTimeNpc;
    public bool knownNpc;

    public ClueSo neededClue;
    public bool dialogueIfNoClue;
    public string id;
    
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}
