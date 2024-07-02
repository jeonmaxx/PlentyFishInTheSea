using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChoreSo : ScriptableObject
{
    public DaysSo possibleDays;
    public int day;
    public bool done;
    public string description;
    public CharacterSo interviewNpc;
    public AnswerSo additionalAnswer;
    [HideInInspector] public GameObject choreHolder;
    public string id;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}