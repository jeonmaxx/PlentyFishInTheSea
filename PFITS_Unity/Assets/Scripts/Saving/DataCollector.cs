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
            data.saveFileInt = 1;
            SaveData();
        }
    }

    private void SaveData()
    {
        if (data.saveFileInt > 0)
        {
            data.choreDatas = chores.Select(chore => new ChoresData
            {
                choreSoId = chore.id,
                choresDone = chore.done
            }).ToList();

            data.characterDatas = characters.Select(character => new CharacterData
            {
                characterSoId = character.id,
                affinities = character.affinity
            }).ToList();

            data.dialogueDatas = dialogues.Select(dialogue => new DialogueData
            {
                dialogueSoId = dialogue.id,
                answerList = new List<Answer>(dialogue.answers)
            }).ToList();

            SaveGameManager.SaveToJSON<DataToSave>(data, "pfits_" + data.saveFileInt + ".json");
        }
    }

    private void LoadData(int saveFileIndex)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "pfits_" + saveFileIndex + ".json");
        DataToSave dataToLoad = SaveGameManager.ReadFromJSON<DataToSave>(filePath);

        if (dataToLoad == null) return;

        LoadDialogueData(dataToLoad.dialogueDatas);
        LoadChoreData(dataToLoad.choreDatas);
        LoadCharacterData(dataToLoad.characterDatas);
    }

    private void LoadDialogueData(List<DialogueData> dialogueDatas)
    {
        if (dialogueDatas == null) return;

        foreach (var loadedDialogue in dialogueDatas)
        {
            var dialogue = dialogues.FirstOrDefault(d => d.id == loadedDialogue.dialogueSoId);
            if (dialogue != null)
            {
                dialogue.answers = new List<Answer>(loadedDialogue.answerList);
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
