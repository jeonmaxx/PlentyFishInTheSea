using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnDoor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject currentRoom;
    public GameObject nextRoom;
    public Renderer rend;
    public RoomManager roomManager;

    private void Start()
    {
        rend.material.SetColor("_BorderColor", Color.yellow);
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
        Debug.Log("clicked on door");
        roomManager.activeRoom = nextRoom;
        Debug.Log(currentRoom.name);
    }
}
