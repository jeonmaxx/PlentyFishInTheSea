using System.Collections.Generic;

[System.Serializable]
public class DataToSave
{
    public int saveFileInt;
    public bool fileChosen;

    //dialogues
    public List<DialogueData> dialogueDatas;

    //characters
    public List<CharacterData> characterDatas;

    //chores
    public List<ChoresData> choreDatas;

    //clues
    public List<bool> cluesNoted;

    //index
    public List<string> knownNpcsIds;
}

[System.Serializable]
public class DialogueData
{
    public string dialogueSoId;
    public List<Answer> answerList;
}

[System.Serializable]
public class CharacterData
{
    public string characterSoId;
    public int affinities;
}

[System.Serializable]
public class ChoresData
{
    public string choreSoId;
    public bool choresDone;
}
