using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; //No se hasta que punto esto va con FMOD, pero ahi está
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class OptionsLoader : MonoBehaviour
{
    public FMOD.Studio.Bus music;
    public FMOD.Studio.Bus sfx;

    //Data
    public Resolution[] resolutions;
    public List<string> resolutionsString;

    //Variables
    public int currentResolutionIndex;
    public int currentMusicVolume;
    public int currentSFX_Volume;
    public bool fullscreen;

    //Default Values
    public bool defaultFullscreenValue = false;
    public int defaultMusicVolume = 50;
    public int defaultSFX_Volume = 50;
    public int[] FPSLimits = { 60, 75, 120, 144 };
    public int FPSLimitIterator = 0;
    public bool vSyncOn = false;
    public bool inputKeyboard = true;
    public int languageIterator = 0;
    public int languageMax; //se asigna en Start

    // Start is called before the first frame update
    public void Init()
    {

        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        resolutionsString = new List<string>();
        resolutions = Screen.resolutions;
        currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            resolutionsString.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        LoadSettings();
    }

    public IEnumerator Start()
    {

        yield return LocalizationSettings.InitializationOperation;
        languageMax = LocalizationSettings.AvailableLocales.Locales.Count - 1;


        //Language

        if (PlayerPrefs.HasKey("LanguagePreference"))
        {
            languageIterator = PlayerPrefs.GetInt("LanguagePreference");
        }
        else
        {
            languageIterator = 0; //el primero que salga
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIterator];
    }

    public void LoadSettings() //Poner carga desde el principio, no que tenga que cargar Options para acceder
    {
        if (PlayerPrefs.HasKey("ResolutionIndexPreference"))
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndexPreference");
        else
            currentResolutionIndex = resolutions.GetLength(0) - 1;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
            fullscreen = Screen.fullScreen;
        }
        else
        {
            Screen.fullScreen = defaultFullscreenValue;
            fullscreen = defaultFullscreenValue;
        }

        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, fullscreen);

        //VSync
        if (PlayerPrefs.HasKey("VerticalSyncPreference"))
        {
            if (PlayerPrefs.GetInt("VerticalSyncPreference") == 1) //VSync si
            {
                vSyncOn = true;
                QualitySettings.vSyncCount = 1;
            }
            else //VSync no
            {
                vSyncOn = false;
                QualitySettings.vSyncCount = 0; //Dont Sync
            }
        }
        else
        {
            //vSync por defecto esta en false
            QualitySettings.vSyncCount = 0;

            //Disable FPS Limit
        }

        //FPS Limit
        if (PlayerPrefs.HasKey("FPSLimitPreference"))
        {
            FPSLimitIterator = PlayerPrefs.GetInt("FPSLimitPreference");
        }
        else
        {
            FPSLimitIterator = FPSLimits.GetLength(0) - 1;
        }

        Application.targetFrameRate = FPSLimits[FPSLimitIterator]; //se aplica en ambos casos

        if (PlayerPrefs.HasKey("MusicVolumePreference"))
            currentMusicVolume = PlayerPrefs.GetInt("MusicVolumePreference");
        else
            currentMusicVolume = defaultMusicVolume;

        //audioMixer.SetFloat("Volume", currentMusicVolume);

        if (PlayerPrefs.HasKey("SFX_VolumePreference"))
            currentSFX_Volume = PlayerPrefs.GetInt("SFX_VolumePreference");
        else
            currentSFX_Volume = defaultSFX_Volume;


        sfx.setVolume(currentSFX_Volume / 100.0f);
        music.setVolume(currentMusicVolume / 100.0f);

        //audioMixer.SetFloat("Volume", currentMusicVolume);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen)); //FullScreen
        PlayerPrefs.SetInt("ResolutionIndexPreference", currentResolutionIndex); //Resolution

        if (vSyncOn)    //Vsync
            PlayerPrefs.SetInt("VerticalSyncPreference", 1);
        else
            PlayerPrefs.SetInt("VerticalSyncPreference", 0);

        PlayerPrefs.SetInt("FPSLimitPreference", FPSLimitIterator); //FPS Limit
        PlayerPrefs.SetInt("MusicVolumePreference", currentMusicVolume); //Music Volume
        PlayerPrefs.SetInt("SFX_VolumePreference", currentSFX_Volume); //SFX Volume

        if (inputKeyboard) //InputMode
            PlayerPrefs.SetInt("InputModePreference", 1);
        else
            PlayerPrefs.SetInt("InputModePreference", 0);

        PlayerPrefs.SetInt("LanguagePreference", languageIterator); //Idioma
    }
}
