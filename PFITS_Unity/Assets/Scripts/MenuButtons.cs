using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public int sceneInt;

    public void LoadSceneButton()
    {
        SceneManager.LoadScene(sceneInt);
    }

    public void EndGameButton()
    {
        Application.Quit();
    }
}
