using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    private const float DEFAULT_MASTER_VOLUME = 0.0f;//-40.0f;
    private const float DEFAULT_EFFECTS_VOLUME = 0.0f;//-20.0f;
    private const float DEFAULT_MUSIC_VOLUME = 0.0f;//-60.0f;
    private const int DEFAULT_RESOLUTION_WIDTH = 1680;
    private const int DEFAULT_RESOLUTION_HEIGHT = 720;
    private const int DEFAULT_FULLSCREEN = 0;

    private List<Resolution> resolutions;// = Resol[];


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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateResolutions()
    {
        Resolution[] originalRes = Screen.resolutions;
        int maxWidth = originalRes[originalRes.Length].width;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < originalRes.Length; i++)
        {
            if (i > 0 && originalRes[i].height != resolutions[i - 1].height)
            {
                int newWidth = (resolutions[i].height * 21) / 9;
                if (newWidth > maxWidth) break;
                resolutions.Add(originalRes[i]);
                //resolutions[i].width = newWidth;
                Debug.Log(resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate);
                string option = resolutions[i].width + " x " + resolutions[i].height;
                resolutionOptions.Add(option);
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
    }

    private void CreateSaveSettings()
    {
        Debug.Log("No PlayerPrefs found! Creating one with default values...");
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
        PlayerPrefs.DeleteAll(); // For testing, remove after!
        if (!PlayerPrefs.HasKey("MusicVolume")) CreateSaveSettings();
        else Debug.Log("PlayerPrefs found!");
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        audioMixer.SetFloat("effectsVolume", PlayerPrefs.GetFloat("EffectsVolume"));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        // Here check saved resolution exists in resolutions and if it does get the index, if not get the index from the nearest resolution
        // Display the id on the dropdawn
        int resolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
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
