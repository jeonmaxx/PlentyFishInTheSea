using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rooms { Art, Outside, Book, Community, Hallway01, Hallway02, Garden, Kitchen, Writing, Science, Sport }
public class RoomDefinition : MonoBehaviour
{
    public Rooms room;
    public string roomName;
}
