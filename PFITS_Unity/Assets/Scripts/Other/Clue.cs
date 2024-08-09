using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clue : MonoBehaviour, IPointerDownHandler
{
    public ClueSo clue;
    public ClueManager clueManager;

    private void Start()
    {
        if(clue.clueNoted)
        {
            GameObject newClue = Instantiate(clueManager.clueText, clueManager.clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
        }

        if ((clue.pickUpItem && clue.clueNoted) || clueManager.dayManager.dayList.days[clueManager.dayManager.currentDayInt] != clue.day)
        {
            transform.localScale = Vector3.zero;
        }
        else
            transform.localScale = Vector3.one;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clueManager.AddClue(clue, gameObject);
    }
}
