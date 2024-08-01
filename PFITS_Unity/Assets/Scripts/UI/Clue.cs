using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clue : MonoBehaviour, IPointerDownHandler
{
    public ClueSo clue;
    public GameObject clueText;
    public GameObject clueNotebook;
    public GameObject itemImage;
    public DayManager dayManager;

    private void Start()
    {
        if(clue.clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
        }

        if ((clue.pickUpItem && clue.clueNoted) || dayManager.dayList.days[dayManager.currentDayInt] != clue.day)
        {
            transform.localScale = Vector3.zero;
        }
        else
            transform.localScale = Vector3.one;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!clue.clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
        }

        if (clue.pickUpItem)
            gameObject.SetActive(false);
        else
        {
            GameObject parentObject = itemImage.transform.parent.gameObject;
            parentObject.SetActive(true);
            itemImage.GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        }
        clue.clueNoted = true;
    }
}
