using System.IO;
using TMPro;
using UnityEngine;

public enum FileAction { Load, Delete }
public class SaveFileButton : MonoBehaviour
{
    public int assignedSaveFile;
    public FileAction fileAction = FileAction.Load;
    public MenuButtons buttons;
    public DataToSave savedData;
    public string filePath;
    private TextMeshProUGUI textObj;
    public DeleteSave deleteSave;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath + "/" + "pfits_" + assignedSaveFile.ToString() + ".json");
        textObj = GetComponentInChildren<TextMeshProUGUI>();
        if(CheckSaveFile())
        {
            Debug.Log(filePath);
            textObj.text = "Savefile 0" + assignedSaveFile;
            savedData = SaveGameManager.ReadFromJSON<DataToSave>("pfits_" + assignedSaveFile.ToString() + ".json");
        }
        else
        {
            textObj.text = "New Game";
        }
    }

    private void Update()
    {
        if (CheckSaveFile())
        {
            textObj.text = "Savefile 0" + assignedSaveFile;
        }
        else
        {
            textObj.text = "New Game";
        }
    }

    private bool CheckSaveFile()
    {
        if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
        {
            return true;
        }
        return false;
    }

    public void SelectFileButton()
    {
        if (fileAction == FileAction.Load)
        {
            if (CheckSaveFile())
            {
                MarkFile();
            }
            else
            {
                NewFile();
            }
        }
        
        if(fileAction == FileAction.Delete)
        {
            deleteSave.gameObject.SetActive(true);
            deleteSave.saveFilePath = filePath;
        }
    }

    private void MarkFile()
    {
        savedData = SaveGameManager.ReadFromJSON<DataToSave>("pfits_" + assignedSaveFile.ToString() + ".json");
        savedData.fileChosen = true;
        SaveGameManager.SaveToJSON(savedData, "pfits_" + assignedSaveFile.ToString() + ".json");
        buttons.LoadSceneButton();
    }

    private void NewFile()
    {
        savedData.fileChosen = true;
        savedData.saveFileInt = assignedSaveFile;
        SaveGameManager.SaveToJSON(savedData, "pfits_" + assignedSaveFile.ToString() + ".json");
        buttons.LoadSceneButton();
    }
}
