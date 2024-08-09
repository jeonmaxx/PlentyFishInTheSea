using System;

[Serializable]
public class PriorityDialogue
{
    public DialogueSo Dialogue { get; }
    public int PriorityLevel { get; }

    public PriorityDialogue(DialogueSo dialogue, int priorityLevel)
    {
        Dialogue = dialogue;
        PriorityLevel = priorityLevel;
    }
}
