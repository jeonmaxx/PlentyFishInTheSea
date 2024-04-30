using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clue : MonoBehaviour, IPointerDownHandler
{
    public GameObject clueText;
    public GameObject clueNotebook;
    public string description;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject newClue = Instantiate(clueText, clueNotebook.transform);
        newClue.GetComponent<TextMeshProUGUI>().text = description;
        Destroy(gameObject);
    }

}
