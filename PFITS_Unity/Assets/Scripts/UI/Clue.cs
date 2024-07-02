using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clue : MonoBehaviour, IPointerDownHandler
{
    public int day;
    public GameObject clueText;
    public GameObject clueNotebook;
    public GameObject itemImage;
    public string description;
    public bool pickUpItem;
    public bool clueNoted;
    public DayManager dayManager;

    private void Start()
    {
        if(clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = description;
        }

        if ((pickUpItem && clueNoted) || dayManager.dayList.days[dayManager.currentDayInt] != day)
        {
            transform.localScale = Vector3.zero;
        }
        else
            transform.localScale = Vector3.one;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = description;
        }

        if (pickUpItem)
            gameObject.SetActive(false);
        else
        {
            GameObject parentObject = itemImage.transform.parent.gameObject;
            parentObject.SetActive(true);
            itemImage.GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        }
        clueNoted = true;
    }
}
