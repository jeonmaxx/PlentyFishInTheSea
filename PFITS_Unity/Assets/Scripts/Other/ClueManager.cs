using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : MonoBehaviour
{
    public GameObject clueText;
    public GameObject clueNotebook;
    public GameObject itemImage;
    public DayManager dayManager;

    public List<ClueSo> allClues;
    public List<ClueSo> foundClues;

    public void AddClue(ClueSo clue, GameObject clueObject)
    {
        if (!clue.clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
        }

        if (clueObject != null)
        {
            if (clue.pickUpItem)
                clueObject.SetActive(false);

            else
            {
                GameObject parentObject = itemImage.transform.parent.gameObject;
                parentObject.SetActive(true);
                itemImage.GetComponent<Image>().sprite = clue.clueSprite;
            }
        }
        clue.clueNoted = true;
        foundClues.Add(clue);
    }
}
