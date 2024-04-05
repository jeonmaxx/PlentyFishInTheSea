using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNpc : PlayerNear
{
    public DialogueTrigger trigger;

    public InputActionReference inputAction;
    private InputAction action;

    public void Start()
    {
        action = inputAction.action;
    }

    private void Update()
    {
        CalcDistance();
        action.started += _ => OnInteract();
    }

    public void OnInteract()
    {
        if (isPlayerNear)
        {
            trigger.StartDialogue();
            Debug.Log(trigger.messages.Length);
        }
    }

    
}
