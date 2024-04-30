using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Renderer rend;
    public bool isPerson = true;
    public GameObject dialogueWindow;
    public DialogueTrigger trigger;

    private void Start()
    {
        rend.material.SetColor("_BorderColor", Color.blue);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if(isPerson)
        {
            dialogueWindow.transform.localScale = Vector3.one;
            trigger.StartDialogue();
        }
    }
}
