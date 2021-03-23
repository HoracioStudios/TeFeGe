using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Audio; //No se hasta que punto esto va con FMOD, pero ahi está
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer; //FMOD???

    //Data
    Resolution[] resolutions;
    List<string> resolutionsString;
    private string[] inputModes = { "Teclado, Mando" }; //Temporal, tengo que meter lo de la traduccion

    //Variables
    int currentResolutionIndex;
    int currentMusicVolume;
    int currentSFX_Volume;
    bool fullscreen;

    //Default Values
    bool defaultFullscreenValue = false;
    int defaultMusicVolume = 50;
    int defaultSFX_Volume = 50;
    int[] FPSLimits = {60, 75, 120, 144};
    int FPSLimitIterator = 0;
    bool vSyncOn = false;
    bool inputKeyboard = true;
    int languageIterator = 0;
    int languageMax; //se asigna en Start

    //Texts
    public Text fullScreen;
    public Text resolution;
    public Text musicVolume;
    public Text sfxVolume;
    public Text inputMode;
    public Text language;
    public Text FPSLimit;
    public Text VSync;

    // Start is called before the first frame update
    void Start()
    {
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

        LoadSettings(currentResolutionIndex);
        StartText();

        languageMax = LocalizationSettings.AvailableLocales.Locales.Count - 1;
    }

    protected void StartText()
    {
        FPSLimit.text = FPSLimits[FPSLimitIterator].ToString();

        musicVolume.text = currentMusicVolume.ToString();

        sfxVolume.text = currentSFX_Volume.ToString();

        UpdateInputModeText(inputKeyboard);

        updateLanguageText();
    }

    public void changeFullScreen()
    {
        fullscreen = !fullscreen;

        if (fullscreen)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }

        Screen.fullScreen = fullscreen;
    }

    public void lessResolution()
    {
        if (currentResolutionIndex > 0)
            currentResolutionIndex -= 1;
        else
            currentResolutionIndex = resolutions.GetLength(0) - 1;

        SetResolution(currentResolutionIndex);
        resolution.text = resolutionsString[currentResolutionIndex];
    }

    public void moreResolution()
    {
        if (currentResolutionIndex < (resolutions.GetLength(0) - 1))
            currentResolutionIndex += 1;
        else
            currentResolutionIndex = 0;

        SetResolution(currentResolutionIndex);
        resolution.text = resolutionsString[currentResolutionIndex];
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void changeVSync()
    {
        if (vSyncOn) //Pasa a false
        {
            QualitySettings.vSyncCount = 0; //Dont Sync
            vSyncOn = false;

            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }

            //Enable de FPSLimit
        }
        else //Pasa a true
        {
            QualitySettings.vSyncCount = 1; //Sync a lo que vaya el monitor
            vSyncOn = true;

            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }

            //Disable de FPSLimit
        }
    }

    public void moreFPSLimit()
    {
        if (FPSLimitIterator < FPSLimits.GetLength(0) - 1)
        {
            FPSLimitIterator += 1;
        }
        else
        {
            FPSLimitIterator = 0;
        }

        Application.targetFrameRate = FPSLimits[FPSLimitIterator];

        updateFPSLimitText();
    }

    public void lessFPSLimit()
    {
        if (FPSLimitIterator > 0)
        {
            FPSLimitIterator -= 1;
        }
        else
        {
            FPSLimitIterator = FPSLimits.GetLength(0) - 1;
        }

        Application.targetFrameRate = FPSLimits[FPSLimitIterator];
        
        updateFPSLimitText();
    }

    void updateFPSLimitText()
    {
        FPSLimit.text = (FPSLimits[FPSLimitIterator]).ToString();
    }

    public void moreSFX_Volume()
    {
        if (currentSFX_Volume <= 95)
        {
            currentSFX_Volume += 5;
            //audioMixer.SetFloat("Volume", currentSFX_Volume); //FMOD, o distinto parametro??

            sfxVolume.text = currentSFX_Volume.ToString(); //UpdateText
        }
        else
        {

        }
    }

    public void lessSFX_Volume()
    {
        if (currentSFX_Volume >= 5)
        {
            currentSFX_Volume -= 5;
            //audioMixer.SetFloat("Volume", currentSFX_Volume); //FMOD, o distinto parametro??

            sfxVolume.text = currentSFX_Volume.ToString(); //UpdateText
        }
        else
        {

        }
    }

    public void moreMusicVolume()
    {
        if (currentMusicVolume <= 95)
        {
            currentMusicVolume += 5;
            //audioMixer.SetFloat("Volume", currentMusicVolume); //FMOD, o distinto parametro??

            musicVolume.text = currentMusicVolume.ToString(); //UpdateText
        }
        else 
        {
        
        }
    }

    public void lessMusicVolume()
    {
        if (currentMusicVolume >= 5)
        {
            currentMusicVolume -= 5;
            //audioMixer.SetFloat("Volume", currentMusicVolume); //FMOD, o distinto parametro??

            musicVolume.text = currentMusicVolume.ToString(); //UpdateText
        }
        else
        {

        }
    }

    public void changeInputMode()
    {
        if (inputKeyboard) //Pasa a controller/false
        {
            //Cosa de gameManager para el input

            //Cambio de texto

            inputKeyboard = false;
        }
        else //Pasa a keyboard/true
        {
            //Cosa de gameManager para el input

            //Cambio de texto
            inputKeyboard = true;
        }

        UpdateInputModeText(inputKeyboard);
    }

    void UpdateInputModeText(bool keyboard)
    {
        if (keyboard)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Keyboard");

            if (test.IsDone)
            {
                inputMode.text = test.Result;
            }
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Controller");

            if (test.IsDone)
            {
                inputMode.text = test.Result;
            }
        }
    }

    public void lessLanguage()
    {
        if (languageIterator > 0)
        {
            languageIterator -= 1;
        }
        else
        {
            languageIterator = languageMax;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIterator];

        updateLanguageText();
    }

    public void moreLanguage()
    {
        if (languageIterator < languageMax)
        {
            languageIterator += 1;
        }
        else
        {
            languageIterator = 0;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIterator];

        updateLanguageText();
    }

    //Las 1/2 primeras veces que se actualiza el texto va regular, luego bien??
    //No se si es por el aSync o otra cosa, asi que habrá que testear

    //Hay veces que falla por el aSync 100% (a veces, si se pone FullScreen a "Si" y se cambia de idioma, se queda en "Si" en vez de "Yes"
    void updateLanguageText()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])//Ingles
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "English");

            if (test.IsDone)
            {
                language.text = test.Result;
            }
        }
        else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]) //Español
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Castellano");

            if (test.IsDone)
            {
                language.text = test.Result;
            }
        }

        if (fullscreen)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }

        resolution.text = resolutionsString[currentResolutionIndex];

        if (vSyncOn)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }

        UpdateInputModeText(inputKeyboard);
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

    public void LoadSettings(int currentResolutionIndex) //Poner carga desde el principio, no que tenga que cargar Options para acceder
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

        //audioMixer.SetFloat("Volume", currentMusicVolume);

        //InputMode

        if (PlayerPrefs.HasKey("InputModePreference"))
        {
            if (PlayerPrefs.GetInt("InputModePreference") == 1) //Keyboard
            {
                inputKeyboard = true;
            }
            else //0
            {
                inputKeyboard = false;
            }
        }
        else
        {
            //inputKeyboard es true
        }

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
}
