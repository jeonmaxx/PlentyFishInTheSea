using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SaveFileButton : MonoBehaviour
{
    public int assignedSaveFile;

    private void Start()
    {
        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();
        if(CheckSaveFile())
        {
            tmp.text = "Savefile 0" + assignedSaveFile;
        }
        else
        {
            tmp.text = "New Game";
        }
    }
    private bool CheckSaveFile()
    {
        string filePath = Application.persistentDataPath + "/" + "pfits_" + assignedSaveFile + ".json";
        Debug.Log(filePath);

        if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
        {
            return true;
        }
        return false;
    }
}
