using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSo : ScriptableObject
{
    public int affinity;
    public Actor[] actors;
    public Message[] messages;
}
