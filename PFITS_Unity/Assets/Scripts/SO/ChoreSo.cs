using System.Collections.Generic;
using UnityEngine;

public enum ChoreType { InterviewOne, InterviewNumber, ObjectSearch}
[CreateAssetMenu]
public class ChoreSo : ScriptableObject
{
    public DaysSo possibleDays;
    public int day;
    public bool done;
    public string description;
    public ChoreType type;
    public int npcsToInterview;
    public int currentInterviewed;
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