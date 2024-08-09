using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSo : ScriptableObject
{
    public Actor actor;
    public int affinity;
    [HideInInspector] public string lastRoom;
    public string id;
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}
