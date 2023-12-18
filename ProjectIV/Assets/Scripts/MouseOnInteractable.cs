using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnInteractable : PlayerNear, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Renderer rend;
    public GameObject dialogueWindow;
    public DialogueTrigger trigger;

    private void Update()
    {
        CalcDistance();

        if (isPlayerNear)
            rend.material.SetColor("_BorderColor", Color.blue);
        else
            rend.material.SetColor("_BorderColor", Color.red);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 1);

        Debug.Log("Ohhh chick ;)");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);

        Debug.Log("No chick :(");
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (isPlayerNear)
        {
            dialogueWindow.transform.localScale = Vector3.one;
            trigger.StartDialogue();
        }
    }
}
