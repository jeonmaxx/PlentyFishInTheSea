using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNpc : MonoBehaviour
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
        action.started += _ => OnInteract();
    }

    public void OnInteract()
    {
        trigger.StartDialogue();
        Debug.Log(trigger.messages.Length);
    }    
}
