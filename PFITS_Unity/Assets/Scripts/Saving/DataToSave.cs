using System.Collections.Generic;
using UnityEngine;

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
    public List<ClueData> clueDatas;

    //answers
    public List<AnswerData> answerDatas;

    //index
    public List<string> knownNpcsIds;

    public Rooms activeRoom;
}

[System.Serializable]
public class DialogueData
{
    public string dialogueSoId;
    public bool knownRoom;
    public bool knownNpc;
    public bool diaDone;
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
    public int currentInterviewed;
    public bool noted;
}

[System.Serializable]
public class ClueData
{
    public string clueSoId;
    public bool clueNoted;
}

[System.Serializable]
public class AnswerData
{
    public string answerSoId;
    public bool clicked;
}
