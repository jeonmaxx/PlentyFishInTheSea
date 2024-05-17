using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSo : ScriptableObject
{
    public int affinity;
    public int day;
    public CharacterSo[] characters;
    public List<Message> messages;
    public List<Answer> answers;
}
