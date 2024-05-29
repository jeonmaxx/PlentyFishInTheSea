using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexManager : MonoBehaviour
{
    private List<CharacterSo> knownNpcs = new List<CharacterSo>();
    public GameObject indexPrefab;
    public Transform indexHolder;

    public void AddIndex(CharacterSo newNpc)
    {
        if (!CheckNewIndex(newNpc))
        {
            GameObject newIndex = Instantiate(indexPrefab, indexHolder);
            newIndex.GetComponent<NpcIndexCard>().pictureHolder.sprite = newNpc.actor.indexSprite;
            knownNpcs.Add(newNpc);
        }
            
    }

    private bool CheckNewIndex(CharacterSo newNpc)
    {
        if(knownNpcs.Count == 0)
            return false;

        foreach (CharacterSo characterSo in knownNpcs)
        {
            if (characterSo == newNpc)
                return true;
        }
        return false;
    }
}
