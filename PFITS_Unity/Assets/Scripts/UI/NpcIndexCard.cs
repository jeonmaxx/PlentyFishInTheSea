using UnityEngine;
using UnityEngine.UI;

public class NpcIndexCard : MonoBehaviour
{
    public Image pictureHolder;
    public CharacterSo currentNpc;
    private NpcIndexInfo indexInfo;

    private void Start()
    {
        indexInfo = FindObjectOfType<NpcIndexInfo>();
    }

    public void Click()
    {
        indexInfo.transform.SetAsLastSibling();
        indexInfo.portrait.sprite = pictureHolder.sprite;
        indexInfo.npcName.text = currentNpc.actor.name;
    }
}
