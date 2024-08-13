using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Article 
{
    public int day;
    public bool clueArticle;
    public string title;
    public List<ClueSo> clues;
    public List<NpcAffinity> npcAffinities;
}

[System.Serializable]
public class NpcAffinity
{
    public CharacterSo character;
    public int addedAffinity;
}
