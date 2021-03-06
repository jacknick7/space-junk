using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    private const float DEFAULT_MASTER_VOLUME = 0.0f;
    private const float DEFAULT_EFFECTS_VOLUME = 0.0f;
    private const float DEFAULT_MUSIC_VOLUME = 0.0f;
    private const int DEFAULT_RESOLUTION_WIDTH = 1680;
    private const int DEFAULT_RESOLUTION_HEIGHT = 720;
    private const int DEFAULT_FULLSCREEN = 0;

    private List<Resolution> resolutions;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        GenerateResolutions();
        GetApplyDisplaySettings();
    }

    private void GenerateResolutions()
    {
        Resolution[] originalRes = Screen.resolutions;
        int maxWidth = originalRes[originalRes.Length - 1].width;
        resolutions = new List<Resolution>();

        int newWidth = (originalRes[0].height * 21) / 9;
        Resolution newRes = originalRes[0];
        newRes.width = newWidth;
        resolutions.Add(newRes);
        for (int i = 1; i < originalRes.Length; i++)
        {
            if (originalRes[i].height != resolutions[resolutions.Count - 1].height)
            {
                newWidth = (originalRes[i].height * 21) / 9;
                if (newWidth > maxWidth) break;
                newRes = originalRes[i];
                newRes.width = newWidth;
                resolutions.Add(newRes);
            }
        }

        List<string> resolutionOptions = new List<string>();
        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;
            resolutionOptions.Add(option);
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
    }

    private void CreateSaveSettings()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("MasterVolume", DEFAULT_MASTER_VOLUME);
        PlayerPrefs.SetFloat("EffectsVolume", DEFAULT_EFFECTS_VOLUME);
        PlayerPrefs.SetFloat("MusicVolume", DEFAULT_MUSIC_VOLUME);
        PlayerPrefs.SetInt("ResolutionWidth", DEFAULT_RESOLUTION_WIDTH);
        PlayerPrefs.SetInt("ResolutionHeight", DEFAULT_RESOLUTION_HEIGHT);
        PlayerPrefs.SetInt("Fullscreen", DEFAULT_FULLSCREEN);
        PlayerPrefs.Save();
    }

    private void GetApplyDisplaySettings()
    {
        if (!PlayerPrefs.HasKey("MusicVolume")) CreateSaveSettings();
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        audioMixer.SetFloat("effectsVolume", PlayerPrefs.GetFloat("EffectsVolume"));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        int resolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == PlayerPrefs.GetInt("ResolutionWidth") && resolutions[i].height == PlayerPrefs.GetInt("ResolutionHeight"))
            {
                resolutionIndex = i;
                break;
            }
        }
        resolutionDropdown.value = resolutionIndex;
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") == 1);
        fullscreenToggle.isOn = isFullscreen;
        Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth"), PlayerPrefs.GetInt("ResolutionHeight"), isFullscreen);
    }

    public void SetApplySaveMasterVolume(float masterVolume)
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        audioMixer.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void SetApplySaveEffectsVolume(float effectsVolume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
        audioMixer.SetFloat("effectsVolume", effectsVolume);
        PlayerPrefs.Save();
    }

    public void SetApplySaveMusicVolume(float musicVolume)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        audioMixer.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetApplySaveResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("ResolutionWidth", resolutions[resolutionIndex].width);
        PlayerPrefs.SetInt("ResolutionHeight", resolutions[resolutionIndex].height);
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") == 1);
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, isFullscreen);
        PlayerPrefs.Save();
    }

    public void SetApplySaveFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        Screen.SetResolution(PlayerPrefs.GetInt("ResolutionWidth"), PlayerPrefs.GetInt("ResolutionHeight"), isFullscreen);
    }
}
