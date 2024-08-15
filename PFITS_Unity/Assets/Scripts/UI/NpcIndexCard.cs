using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcIndexCard : MonoBehaviour
{
    public Image pictureHolder;
    public TextMeshProUGUI npcName;
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
        indexInfo.affinityBar.value = currentNpc.affinity;
        indexInfo.clubMember.text = currentNpc.clubMember;
        indexInfo.lastLocation.text = currentNpc.lastRoom;
    }
}
