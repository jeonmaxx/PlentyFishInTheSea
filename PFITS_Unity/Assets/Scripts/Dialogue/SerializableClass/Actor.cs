using UnityEngine;

[System.Serializable]
public class Actor
{
    [Header ("General")]
    public string name;
    public Sprite indexSprite;
    public Sprite sprite;
    public Sprite happySprite;
    public Sprite angrySprite;

    [Header("Audio")]
    public AudioClip[] dialogueTypingSounds;
    [Range(1, 5)]
    public int frequenzyLevel = 3;
    [Range(-3, 3)]
    public float minPitch = 0.5f;
    [Range(-3, 3)]
    public float maxPitch = 3f;
}
