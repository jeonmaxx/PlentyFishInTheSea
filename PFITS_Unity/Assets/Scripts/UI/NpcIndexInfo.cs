using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcIndexInfo : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI npcName;
    public Slider affinityBar;
    public TextMeshProUGUI lastLocation;

    public void CloseInfo()
    {
        transform.SetAsFirstSibling();
    }
}
