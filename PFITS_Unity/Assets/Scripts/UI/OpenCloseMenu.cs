using UnityEngine;
using UnityEngine.InputSystem;

public class OpenCloseMenu : MonoBehaviour
{
    public bool menuOpen = false;
    public InputActionReference inputAction;
    private InputAction action;

    public void Start()
    {
        transform.localScale = Vector3.zero;
        action = inputAction.action;
    }

    public void Update()
    {
        action.started += _ => OnInteract();

        if (menuOpen)
            transform.localScale = Vector3.one;
        else
            transform.localScale = Vector3.zero;
    }

    public void OpenCloseButton()
    {
        if(menuOpen)
            menuOpen = false;
        else
            menuOpen = true;
    }

    public void OnInteract()
    {
        OpenCloseButton();
    }
}
