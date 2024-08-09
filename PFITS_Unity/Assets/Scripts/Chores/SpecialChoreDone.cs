using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialChoreDone : MonoBehaviour
{
    public ChoreSo chore;
    public MouseOnDoor door;
    public Vector3 newPosition;

    public void Update()
    {
        if(chore.done)
        {
            door.enabled = true;
            gameObject.transform.position = newPosition;
        }
    }
}
