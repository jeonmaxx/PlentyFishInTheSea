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
    public GameObject popUp;
    public RandomSound sounds;
    public float popUpDuration;
    public float openDuration;

    public List<ClueSo> allClues;
    public List<ClueSo> foundClues;

    public void Start()
    {
        foreach (ClueSo clue in allClues)
        {
            if (clue.clueNoted)
            {
                GameObject newClue = Instantiate(clueText, clueNotebook.transform);
                newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
                foundClues.Add(clue);
                if (clue.bookSprite != null)
                {
                    Transform picHolder = newClue.transform.GetChild(0);
                    Instantiate(clue.bookSprite, picHolder);
                }
            }  
        }
    }

    public void AddClue(ClueSo clue, GameObject clueObject)
    {
        if (!clue.clueNoted)
        {
            StartCoroutine(PopUpCoroutine());
            GameObject newClue = Instantiate(clueText, clueNotebook.transform);
            newClue.GetComponent<TextMeshProUGUI>().text = clue.description;
            if (clue.bookSprite != null)
            {
                Transform picHolder = newClue.transform.GetChild(0);
                Instantiate(clue.bookSprite, picHolder);
            }
        }

        if (clueObject != null)
        {
            if (clue.pickUpItem)
                clueObject.SetActive(false);
            else if (!clue.pickUpItem && clueObject.GetComponent<Clue>().clueObject != null)
                clueObject.GetComponent<Clue>().clueObject.SetActive(true);
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

    private IEnumerator PopUpCoroutine()
    {
        popUp.transform.localScale = Vector3.zero;
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = "[ESC] CLUE UPDATE";
        sounds.PlaySound();
        float elapsedTime = 0f;
        while (elapsedTime < popUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Clamp01(elapsedTime / popUpDuration);
            popUp.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        popUp.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(openDuration);

        elapsedTime = 0f;
        while (elapsedTime < popUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Clamp01(1f - (elapsedTime / popUpDuration));
            popUp.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        popUp.transform.localScale = Vector3.zero;
    }
}
