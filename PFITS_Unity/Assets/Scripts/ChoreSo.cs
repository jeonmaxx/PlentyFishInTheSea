using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChoreSo : ScriptableObject
{
    public int day;
    public bool done;
    public string description;
    public CharacterSo interviewNpc;
    public Answer additionalAnswer;
    [HideInInspector] public GameObject choreHolder;
}