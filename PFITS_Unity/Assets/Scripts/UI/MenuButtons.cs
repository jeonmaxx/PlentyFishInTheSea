using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public int sceneInt;
    public int maxSaveFiles = 3;
    public GameObject noEmptyFiles;
    public SaveFileButton[] saveFiles;

    public void LoadSceneButton()
    {
        SceneManager.LoadScene(sceneInt);
    }

    public void EndGameButton()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        int emptyGame = SearchNewGame();

        if(emptyGame > 0)
        {
            DataToSave saveData = new DataToSave();
            saveData.fileChosen = true;
            saveData.saveFileInt = emptyGame;
            SaveGameManager.SaveToJSON(saveData, "pfits_" + emptyGame.ToString() + ".json");
            LoadSceneButton();
        }
        else
        {
            noEmptyFiles.SetActive(true);
        }
    }

    public void DeleteGame()
    {
        foreach(var file in saveFiles)
        {
            file.fileAction = FileAction.Delete;
        }
    }

    public void ResetToLoad()
    {
        foreach (var file in saveFiles)
        {
            file.fileAction = FileAction.Load;
        }
    }

    private int SearchNewGame()
    {
        for (int i = 1; i <= maxSaveFiles; i++)
        {
            string filePath = Application.persistentDataPath + "/" + "pfits_" + (i) + ".json";

            if (!File.Exists(filePath))
            {
                return i;
            }
        }

        return 0;
    }
}
