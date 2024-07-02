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
    public List<AnswerSo> answers;
    public string id;
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}
