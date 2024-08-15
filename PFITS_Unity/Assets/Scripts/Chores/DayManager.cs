using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject mainChoreHolder;
    public GameObject sideChoreHolder;
    public GameObject chorePrefab;
    public RandomSound sound;
    public GameObject popUp;
    public float popUpDuration;
    public float openDuration;

    public GameObject stillToDoScreen;
    public GameObject demoEndScreen;
    private NpcManager npcManager;
    public RoomManager roomManager;
    public GameObject articleScreen;
    public GameObject articleHolder;
    public GameObject articlePrefab;
    public float fadeDuration = 1;
    public List<Article> articles;

    private DateManager dateManager;

    void Start()
    {
        dayText.text = "Day: " + dayList.days[currentDayInt];
        npcManager = GetComponent<NpcManager>();
        dateManager = FindAnyObjectByType<DateManager>();
        AddChores();
    }

    public void AddChores()
    {
        StartCoroutine(PopUpCoroutine());
        if (mainChoreHolder.transform.childCount != 0)
        {
            foreach (Transform child in mainChoreHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (sideChoreHolder.transform.childCount != 0)
        {
            foreach (Transform child in sideChoreHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        choresOfToday = new List<ChoreSo>();
        foreach (ChoreSo chore in chores)
        {
            if (chore.day == dayList.days[currentDayInt] && chore.noted)
            {
                Transform holderTransform = null;

                if (chore.priority == ChorePriority.Main)
                {
                    holderTransform = mainChoreHolder.transform;
                }
                else
                {
                    holderTransform = sideChoreHolder.transform;
                }
                GameObject refChore = Instantiate(chorePrefab, holderTransform);
                refChore.GetComponent<ChoreManager>().toggle.isOn = chore.done;
                if (chore.type == ChoreType.InterviewNumber)
                {
                    refChore.GetComponent<ChoreManager>().description.text = chore.description + " (" + chore.currentInterviewed + "/" + chore.npcsToInterview + ")";
                }
                else
                {
                    refChore.GetComponent<ChoreManager>().description.text = chore.description;
                }
                chore.choreHolder = refChore;
                choresOfToday.Add(chore);
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
            if (chore.day == dayList.days[currentDayInt] && chore.noted)
            {
                chore.choreHolder.GetComponent<ChoreManager>().toggle.isOn = chore.done;
            }
        }

        if (!dateManager.dateInProgress && dateManager.endScreen)
        {
            StartNextDay();
            dateManager.endScreen = false;
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
            roomManager.activeRoom = dateManager.dateRoom;
            dateManager.StartDialogue();
        }
        else
        {
            stillToDoScreen.SetActive(true);
        }
    }

    public void StartNextDay()
    {
        StartCoroutine(FadeToIntensity(0, fadeDuration));
        ShowArticles();
    }

    public void ChoseArticle()
    {
        foreach(Transform transform in articleHolder.transform)
        {
            Destroy(transform.gameObject);
        }
        demoEndScreen.SetActive(true);
    }

    public IEnumerator FadeToIntensity(float targetIntensity, float duration)
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
        articleScreen.SetActive(true);
        foreach (Article article in articles)
        {
            if (article.day == dayList.days[currentDayInt])
            {
                if (article.clueArticle)
                {
                    bool allCluesNoted = true;
                    foreach (ClueSo clue in article.clues)
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
            }
        }
    }

    private IEnumerator PopUpCoroutine()
    {
        popUp.transform.localScale = Vector3.zero;
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = "[ESC] CHORE UPDATE";
        sound.PlaySound();

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
