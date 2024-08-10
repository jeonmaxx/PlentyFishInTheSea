using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnDoor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject currentRoom;
    public GameObject nextRoom;
    private RoomDefinition roomDefinition;
    public Renderer rend;
    public RoomManager roomManager;
    public Texture2D cursor;
    public GameObject roomNameObj;
    public bool cantLeave = false;

    private void Start()
    {
        rend.material.SetColor("_BorderColor", Color.yellow);
        roomDefinition = nextRoom.GetComponent<RoomDefinition>();
        roomNameObj.GetComponent<TextMeshPro>().text = roomDefinition.roomName;
        roomNameObj.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 1);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        roomNameObj.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rend.material.SetInt("_isOn", 0);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        roomNameObj.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(EndFrame());
    }

    public void Leaving()
    {
        if (!cantLeave)
        {
            rend.material.SetInt("_isOn", 0);
            roomNameObj.SetActive(false);
            roomManager.activeRoom = nextRoom;
        }
    }

    private IEnumerator EndFrame()
    {
        yield return new WaitForEndOfFrame();
        Leaving();
    }
}
