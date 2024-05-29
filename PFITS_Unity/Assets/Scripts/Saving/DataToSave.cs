using System.Collections.Generic;

[System.Serializable]
public class DataToSave
{
    public int saveFileInt;
    public bool fileChosen;

    //saving for dialogues
    public List<DialogueData> dialogueDatas;

    //saving for characters
    public List<CharacterData> characterDatas;

    //saving for chores
    public List<ChoresData> choreDatas;
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
