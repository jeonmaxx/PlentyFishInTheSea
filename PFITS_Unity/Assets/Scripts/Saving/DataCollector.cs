using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    [Header("Input")]
    public int maxSaveFiles = 3;
    public List<DialogueSo> dialogues;
    public List<CharacterSo> characters;
    public List<ChoreSo> chores;
    public List<ClueSo> clues;
    public List<AnswerSo> answers;
    public IndexManager indexManager;
    public RoomManager roomManager;

    [Header("Saved Data")]
    public DataToSave data;

    private void Awake()
    {
        string filePath = Application.persistentDataPath;
        Debug.Log(filePath);

        int saveFileIndex = CheckSaveFiles();

        if (saveFileIndex != 0)
        {
            data.saveFileInt = saveFileIndex;
            LoadData(saveFileIndex);
        }
        else
        {
            data.activeRoom = Rooms.Writing;
            data.saveFileInt = 1;
            SaveData();
        }
    }

    public void SaveData()
    {
        if (data.saveFileInt > 0)
        {
            data.choreDatas = chores.Select(chore => new ChoresData
            {
                choreSoId = chore.id,
                choresDone = chore.done,
                currentInterviewed = chore.currentInterviewed,
                noted = chore.noted
            }).ToList();

            data.characterDatas = characters.Select(character => new CharacterData
            {
                characterSoId = character.id,
                affinities = character.affinity
            }).ToList();

            data.dialogueDatas = dialogues.Select(dialogue => new DialogueData
            {
                dialogueSoId = dialogue.id,
                knownRoom = dialogue.knownRoom,
                knownNpc = dialogue.knownNpc,
                diaDone = dialogue.diaDone
            }).ToList();

            data.clueDatas = clues.Select(clue => new ClueData
            {
                clueSoId = clue.id,
                clueNoted = clue.clueNoted
            }).ToList();

            data.answerDatas = answers.Select(answer => new AnswerData
            {
                answerSoId = answer.id,
                clicked = answer.clicked
            }).ToList();

            data.knownNpcsIds = indexManager.knownNpcs.Select(npc => npc.id).ToList();

            data.activeRoom = roomManager.activeRoom.GetComponent<RoomDefinition>().room;

            SaveGameManager.SaveToJSON<DataToSave>(data, "pfits_" + data.saveFileInt + ".json");
        }
    }

    private void LoadData(int saveFileIndex)
    {
        DataToSave dataToLoad = SaveGameManager.ReadFromJSON<DataToSave>("pfits_" + saveFileIndex + ".json");

        if (dataToLoad == null) return;

        LoadDialogueData(dataToLoad.dialogueDatas);
        LoadChoreData(dataToLoad.choreDatas);
        LoadCharacterData(dataToLoad.characterDatas);
        LoadClueData(dataToLoad.clueDatas);
        LoadAnswerData(dataToLoad.answerDatas);
        LoadKnownNpcsData(dataToLoad.knownNpcsIds);

        foreach(GameObject room in roomManager.rooms)
        {
            Rooms toCheck = room.GetComponent<RoomDefinition>().room;
            if(toCheck == dataToLoad.activeRoom && dataToLoad.activeRoom != 0)
            {
                roomManager.activeRoom = room;
            }
        }
    }

    private void LoadDialogueData(List<DialogueData> dialogueDatas)
    {
        if (dialogueDatas == null) return;

        foreach (var loadedDialogue in dialogueDatas)
        {
            var dialogue = dialogues.FirstOrDefault(d => d.id == loadedDialogue.dialogueSoId);
            if (dialogue != null)
            {
                dialogue.knownRoom = loadedDialogue.knownRoom;
                dialogue.knownNpc = loadedDialogue.knownNpc;
                dialogue.diaDone = loadedDialogue.diaDone;
            }
        }
    }

    private void LoadChoreData(List<ChoresData> choreDatas)
    {
        if (choreDatas == null) return;

        foreach (var loadedChore in choreDatas)
        {
            var chore = chores.FirstOrDefault(c => c.id == loadedChore.choreSoId);
            if (chore != null)
            {
                chore.done = loadedChore.choresDone;
                chore.currentInterviewed = loadedChore.currentInterviewed;
                chore.noted = loadedChore.noted;
            }
        }
    }

    private void LoadCharacterData(List<CharacterData> characterDatas)
    {
        if (characterDatas == null) return;

        foreach (var loadedCharacter in characterDatas)
        {
            var character = characters.FirstOrDefault(c => c.id == loadedCharacter.characterSoId);
            if (character != null)
            {
                character.affinity = loadedCharacter.affinities;
            }
        }
    }

    private void LoadClueData(List<ClueData> clueDatas)
    {
        if (clueDatas == null) return;

        foreach (var clueData in clueDatas)
        {
            var clue = clues.FirstOrDefault(c => c.id == clueData.clueSoId);
            if (clue != null)
            {
                clue.clueNoted = clueData.clueNoted;
            }
        }
    }

    private void LoadAnswerData(List<AnswerData> answerDatas)
    {
        if (answerDatas == null) return;

        foreach (var loadedAnswer in answerDatas)
        {
            var answer = answers.FirstOrDefault(a => a.id == loadedAnswer.answerSoId);
            if (answer != null)
            {
                answer.clicked = loadedAnswer.clicked;
            }
        }
    }

    private void LoadKnownNpcsData(List<string> knownNpcsIds)
    {
        if (knownNpcsIds == null) return;

        indexManager.knownNpcs.Clear();
        foreach (var npcId in knownNpcsIds)
        {
            var character = characters.FirstOrDefault(c => c.id == npcId);
            if (character != null)
            {
                indexManager.knownNpcs.Add(character);
            }
        }
        indexManager.RefreshIndexes();
    }

    private int CheckSaveFiles()
    {
        List<DataToSave> tmpData = new List<DataToSave>();
        for (int i = 1; i <= maxSaveFiles; i++)
        {
            string filePath = Application.persistentDataPath + "/" + "pfits_" + (i) + ".json";

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                DataToSave tmp = SaveGameManager.ReadFromJSON<DataToSave>("pfits_" + (i) + ".json");
                if (tmp != null)
                {
                    tmpData.Add(tmp);
                }
            }
        }

        for (int i = 0; i < tmpData.Count; i++)
        {
            if (tmpData[i].fileChosen)
            {
                Debug.Log("Chosen File: " + i);
                return i + 1;
            }
        }
        Debug.Log("No File found");
        return 0;
    }

    private void OnApplicationQuit()
    {
        SaveData();
        ResetFileChosenFlags();
    }

    private void ResetFileChosenFlags()
    {
        for (int i = 1; i <= maxSaveFiles; i++)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "pfits_" + i + ".json");
            if (File.Exists(filePath))
            {
                DataToSave tmp = SaveGameManager.ReadFromJSON<DataToSave>(filePath);
                if (tmp != null && tmp.fileChosen)
                {
                    tmp.fileChosen = false;
                    SaveGameManager.SaveToJSON(tmp, "pfits_" + i + ".json");
                }
            }
        }
    }
}
