using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour
{
    public DaysSo dayList;
    public int currentDayInt;
    public Light2D lightObj;
    public TextMeshProUGUI dayText;
    public List<ChoreSo> chores;
    public List<ChoreSo> choresOfToday;
    public GameObject choreHolder;
    public GameObject chorePrefab;
    public GameObject stillToDoScreen;
    public GameObject demoEndScreen;
    private NpcManager npcManager;
    public GameObject articleHolder;
    public GameObject articlePrefab;
    public float fadeDuration = 1;
    public List<Article> articles;

    void Start()
    {
        dayText.text = "Day: " + dayList.days[currentDayInt];
        npcManager = GetComponent<NpcManager>();
        AddChores();
    }

    private void AddChores()
    {
        foreach (ChoreSo chore in chores)
        {
            if (chore.day == dayList.days[currentDayInt])
            {
                GameObject refChore = Instantiate(chorePrefab, choreHolder.transform);
                refChore.GetComponent<ChoreManager>().toggle.isOn = chore.done;
                refChore.GetComponent<ChoreManager>().description.text = chore.description;
                chore.choreHolder = refChore;
                choresOfToday.Add(chore);

                for (int i = 0; i < npcManager.npcObjects.Count; i++)
                {
                    if (npcManager.npcObjects[i].character == chore.interviewNpc && !chore.done)
                    {
                        foreach (DialogueSo dialogueSo in npcManager.npcObjects[i].characterObj.GetComponent<DialogueTrigger>().dialogueSo)
                        {
                            if (!CheckAnswer(chore.additionalAnswer, dialogueSo.answers))
                            {
                                dialogueSo.answers.Add(chore.additionalAnswer);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool CheckAnswer(AnswerSo checkAnswer, List<AnswerSo> answers)
    {
        if(answers.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < answers.Count; i++)
        {
            if (answers[i].id == checkAnswer.id)
            {
                Debug.Log("AnswerSo already there");
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        foreach(ChoreSo chore in chores)
        {
            if (chore.day == dayList.days[currentDayInt])
            {
                chore.choreHolder.GetComponent<ChoreManager>().toggle.isOn = chore.done;
            }
        }
    }

    public void EndDayButton()
    {
        List<ChoreSo> choresDone = new List<ChoreSo>();

        foreach(ChoreSo chore in choresOfToday)
        {
            if(chore.done)
                choresDone.Add(chore);
        }

        if(choresDone.Count == choresOfToday.Count)
        {
            StartNextDay();
        }
        else
        {
            stillToDoScreen.SetActive(true);
        }
    }

    public void StartNextDay()
    {
        if (currentDayInt < dayList.days.Count-1)
        {
            StartCoroutine(FadeToIntensity(0, fadeDuration)); 
            ShowArticles();
        }
        else
        {
            demoEndScreen.SetActive(true);
        }
    }

    public void ChoseArticle()
    {
        choresOfToday = new List<ChoreSo>();
        foreach (Transform child in choreHolder.transform)
        {
            Destroy(child.gameObject);
        }
        currentDayInt++;
        dayText.text = "Day: " + dayList.days[currentDayInt];
        AddChores();
        StartCoroutine(FadeToIntensity(1, fadeDuration));
        foreach(Transform transform in articleHolder.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    private IEnumerator FadeToIntensity(float targetIntensity, float duration)
    {
        float startIntensity = lightObj.intensity;
        float time = 0;

        while (time < duration)
        {
            lightObj.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        lightObj.intensity = targetIntensity;
    }

    private void ShowArticles()
    {
        articleHolder.SetActive(true);

        foreach (Article article in articles)
        {
            if (article.day == dayList.days[currentDayInt])
            {
                if (article.clueArticle)
                {
                    bool allCluesNoted = true;
                    foreach (Clue clue in article.clues)
                    {
                        if (!clue.clueNoted)
                        {
                            allCluesNoted = false;
                            break;
                        }
                    }

                    if (!allCluesNoted)
                    {
                        continue;
                    }
                }

                GameObject articleObjInstance = Instantiate(articlePrefab, articleHolder.transform);
                ArticleObject articleObj = articleObjInstance.GetComponent<ArticleObject>();
                articleObj.title.text = article.title;
                articleObj.content.text = article.text;
            }
        }
    }
}
