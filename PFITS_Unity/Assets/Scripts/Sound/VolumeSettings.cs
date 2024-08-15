using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider ambienteVolume;
    [SerializeField] private Slider sfxVolume;

    private void Start()
    {
        if(PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetMusicVolume();
            SetAmbienteVolume();
            SetSfxVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = masterVolume.value;
        audioMixer.SetFloat("masterMix", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicVolume.value;
        audioMixer.SetFloat("musicMix", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetAmbienteVolume()
    {
        float volume = ambienteVolume.value;
        audioMixer.SetFloat("ambienteMix", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ambienteVolume", volume);
    }

    public void SetSfxVolume()
    {
        float volume = sfxVolume.value;
        audioMixer.SetFloat("sfxMix", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        masterVolume.value = PlayerPrefs.GetFloat("masterVolume");
        SetMasterVolume();
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
        ambienteVolume.value = PlayerPrefs.GetFloat("ambienteVolume");
        SetAmbienteVolume();
        sfxVolume.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSfxVolume();
    }
}
