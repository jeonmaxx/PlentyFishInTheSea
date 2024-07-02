using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnDoor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject currentRoom;
    public GameObject nextRoom;
    public Renderer rend;
    public RoomManager roomManager;
    public Texture2D cursor;

    private void Start()
    {
        rend.material.SetColor("_BorderColor", Color.yellow);
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
        Debug.Log("clicked on door");
        roomManager.activeRoom = nextRoom;
        Debug.Log(currentRoom.name);
    }
}
