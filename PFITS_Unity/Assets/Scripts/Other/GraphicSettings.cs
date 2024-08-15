using UnityEngine;
using UnityEngine.UI;

public class GraphicSettings : MonoBehaviour
{
    [SerializeField] private Toggle windowMode;
    [SerializeField] private Toggle fullScreenMode;

    private const string GraphicsKey = "graphics";

    private void Start()
    {
        if (PlayerPrefs.HasKey(GraphicsKey))
        {
            LoadScreenMode();
        }
        else
        {
            SetFullScreen(true);
            PlayerPrefs.SetInt(GraphicsKey, 1);
            PlayerPrefs.Save();
        }

        windowMode.onValueChanged.AddListener(OnWindowToggleChanged);
        fullScreenMode.onValueChanged.AddListener(OnFullScreenToggleChanged);
    }

    public void SetWindow(bool updateToggles = false)
    {
        Screen.fullScreen = false;
        PlayerPrefs.SetInt(GraphicsKey, 0);
        PlayerPrefs.Save();

        if (updateToggles)
        {
            windowMode.SetIsOnWithoutNotify(true);
            fullScreenMode.SetIsOnWithoutNotify(false);
        }
    }

    public void SetFullScreen(bool updateToggles = false)
    {
        Screen.fullScreen = true;
        PlayerPrefs.SetInt(GraphicsKey, 1);
        PlayerPrefs.Save();

        if (updateToggles)
        {
            windowMode.SetIsOnWithoutNotify(false);
            fullScreenMode.SetIsOnWithoutNotify(true);
        }
    }

    private void LoadScreenMode()
    {
        int screenMode = PlayerPrefs.GetInt(GraphicsKey);

        if (screenMode == 0)
        {
            SetWindow(true);
        }
        else
        {
            SetFullScreen(true);
        }
    }

    private void OnWindowToggleChanged(bool isOn)
    {
        if (isOn)
        {
            SetWindow(true);
        }
        else
        {
            SetFullScreen(true);
        }
    }

    private void OnFullScreenToggleChanged(bool isOn)
    {
        if (isOn)
        {
            SetFullScreen(true);
        }
        else
        {
            SetWindow(true);
        }
    }
}
