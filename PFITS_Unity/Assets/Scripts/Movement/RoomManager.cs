using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject activeRoom;
    public List<GameObject> rooms;

    public void Update()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            GameObject room = rooms[i];
            if (room == activeRoom)
                room.SetActive(true);
            else
                room.SetActive(false);
        }
    }
}
