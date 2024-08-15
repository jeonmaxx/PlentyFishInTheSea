using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexManager : MonoBehaviour
{
    public List<CharacterSo> knownNpcs = new List<CharacterSo>();
    public GameObject indexPrefab;
    public Transform indexHolder;
    public CharacterSo player;

    public void AddIndex(CharacterSo newNpc)
    {
        if (!CheckNewIndex(newNpc) && newNpc != player)
        {
            GameObject newIndex = Instantiate(indexPrefab, indexHolder);
            newIndex.GetComponent<NpcIndexCard>().pictureHolder.sprite = newNpc.actor.indexSprite;
            newIndex.GetComponent<NpcIndexCard>().npcName.text = newNpc.actor.name;
            newIndex.GetComponent<NpcIndexCard>().currentNpc = newNpc;
            knownNpcs.Add(newNpc);
        }   
    }

    public void RefreshIndexes()
    {
        foreach (CharacterSo newNpc in knownNpcs)
        {
            GameObject newIndex = Instantiate(indexPrefab, indexHolder);
            newIndex.GetComponent<NpcIndexCard>().pictureHolder.sprite = newNpc.actor.indexSprite;
            newIndex.GetComponent<NpcIndexCard>().npcName.text = newNpc.actor.name;
            newIndex.GetComponent<NpcIndexCard>().currentNpc = newNpc;
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
