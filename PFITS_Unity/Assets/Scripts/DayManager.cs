using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public int currentDay;
    public TextMeshProUGUI dayText;
    public List<Chore> chores;
    public GameObject choreHolder;
    public GameObject chorePrefab;

    void Start()
    {
        dayText.text = "Day: " + currentDay;
        NpcManager npcManager = GetComponent<NpcManager>();
        foreach (Chore chore in chores)
        {
            GameObject refChore = Instantiate(chorePrefab, choreHolder.transform);
            refChore.GetComponent<ChoreManager>().toggle.isOn = chore.done;
            refChore.GetComponent<ChoreManager>().description.text = chore.description;
            chore.choreHolder = refChore;

            for(int i = 0; i < npcManager.npcObjects.Count; i++)
            {
                if (npcManager.npcObjects[i].character == chore.interviewNpc)
                {
                    npcManager.npcObjects[i].characterObj.GetComponent<DialogueTrigger>().dialogueSo[0].answers.Add(chore.additionalAnswer);
                }
            }
        }
    }

    void Update()
    {
        foreach(Chore chore in chores)
        {
            chore.choreHolder.GetComponent<ChoreManager>().toggle.isOn = chore.done;
        }
    }

    public void EndDayButton()
    {

    }
}
