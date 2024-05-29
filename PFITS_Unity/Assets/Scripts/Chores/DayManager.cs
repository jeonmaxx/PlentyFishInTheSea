using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public int currentDay;
    public TextMeshProUGUI dayText;
    public List<ChoreSo> chores;
    public GameObject choreHolder;
    public GameObject chorePrefab;
    public GameObject stillToDoScreen;
    public GameObject demoEndScreen;

    void Start()
    {
        dayText.text = "Day: " + currentDay;
        NpcManager npcManager = GetComponent<NpcManager>();
        foreach (ChoreSo chore in chores)
        {
            if (chore.day == currentDay)
            {
                GameObject refChore = Instantiate(chorePrefab, choreHolder.transform);
                refChore.GetComponent<ChoreManager>().toggle.isOn = chore.done;
                refChore.GetComponent<ChoreManager>().description.text = chore.description;
                chore.choreHolder = refChore;

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

    private bool CheckAnswer(Answer checkAnswer, List<Answer> answers)
    {
        if(answers.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < answers.Count; i++)
        {
            if (answers[i].nextDialogue == checkAnswer.nextDialogue)
            {
                Debug.Log("Answer already there");
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        foreach(ChoreSo chore in chores)
        {
            if (chore.day == currentDay)
            {
                chore.choreHolder.GetComponent<ChoreManager>().toggle.isOn = chore.done;
            }
        }
    }

    public void EndDayButton()
    {
        List<ChoreSo> choresDone = new List<ChoreSo>();

        foreach(ChoreSo chore in chores)
        {
            if(chore.done)
                choresDone.Add(chore);
        }

        if(choresDone.Count == chores.Count)
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
        demoEndScreen.SetActive(true);
    }
}
