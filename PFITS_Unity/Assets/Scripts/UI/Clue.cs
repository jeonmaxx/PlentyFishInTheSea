using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clue : MonoBehaviour, IPointerDownHandler
{
    public GameObject clueText;
    public GameObject clueNotebook;
    public GameObject itemImage;
    public string description;
    public bool pickUpItem;
    private bool clueNoted;

    private void Start()
    {
        //muss noch ins Saving eingebaut werden
        if(pickUpItem && clueNoted)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!clueNoted)
        {
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = description;
        }
        if (pickUpItem)
            Destroy(gameObject);
        else
        {
            GameObject parentObject = itemImage.transform.parent.gameObject;
            parentObject.SetActive(true);
            itemImage.GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        }
        clueNoted = true;
    }
}
