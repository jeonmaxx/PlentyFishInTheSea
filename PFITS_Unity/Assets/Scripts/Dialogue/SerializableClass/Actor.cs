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
    public Sprite sadSprite;
    public Sprite disgustSprite;
    public Sprite fearSprite;
    public Sprite surpriseSprite;
    public Sprite evilSprite;
    public Sprite excitedSprite;

    [Header("Audio")]
    public AudioClip[] dialogueTypingSounds;
    [Range(1, 5)]
    public int frequenzyLevel = 3;
    [Range(-3, 3)]
    public float minPitch = 0.5f;
    [Range(-3, 3)]
    public float maxPitch = 3f;
}
