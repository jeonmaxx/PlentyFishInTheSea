using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClueSo : ScriptableObject
{
    public int day;
    public string description;
    public bool pickUpItem;
    public bool clueNoted;
    public string id;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }
}

