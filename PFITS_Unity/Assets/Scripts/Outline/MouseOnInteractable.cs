using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Renderer rend;
    public bool isPerson = true;
    public GameObject dialogueWindow;
    public DialogueTrigger trigger;
    public Texture2D cursor;

    private void Start()
    {
        rend.material.SetColor("_BorderColor", Color.blue);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 1);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
