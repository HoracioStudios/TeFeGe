using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Audio; //No se hasta que punto esto va con FMOD, pero ahi está
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    //Texts
    public Text fullScreen;
    public Text resolution;
    public Text musicVolume;
    public Text sfxVolume;
    public Text inputMode;
    public Text language;
    public Text FPSLimit;
    public Text VSync;

    public OptionsLoader options;

    private void Start()
    {
        options = GameManager.instance.optionsLoader;

        StartText();
    }

    protected void StartText()
    {
        FPSLimit.text = options.FPSLimits[options.FPSLimitIterator].ToString();

        musicVolume.text = options.currentMusicVolume.ToString();

        sfxVolume.text = options.currentSFX_Volume.ToString();

        updateLanguageText();
    }

    public void changeFullScreen()
    {
        options.fullscreen = !options.fullscreen;

        if (options.fullscreen)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => fullScreen.text = test.Result;
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (test1) => fullScreen.text = test.Result;
        }

        Screen.fullScreen = options.fullscreen;
    }

    public void lessResolution()
    {
        if (options.currentResolutionIndex > 0)
            options.currentResolutionIndex -= 1;
        else
            options.currentResolutionIndex = options.resolutions.GetLength(0) - 1;

        SetResolution(options.currentResolutionIndex);
        resolution.text = options.resolutionsString[options.currentResolutionIndex];
    }

    public void moreResolution()
    {
        if (options.currentResolutionIndex < (options.resolutions.GetLength(0) - 1))
            options.currentResolutionIndex += 1;
        else
            options.currentResolutionIndex = 0;

        SetResolution(options.currentResolutionIndex);
        resolution.text = options.resolutionsString[options.currentResolutionIndex];
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = options.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void changeVSync()
    {
        if (options.vSyncOn) //Pasa a false
        {
            QualitySettings.vSyncCount = 0; //Dont Sync
            options.vSyncOn = false;

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
            options.vSyncOn = true;

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
        if (options.FPSLimitIterator < options.FPSLimits.GetLength(0) - 1)
        {
            options.FPSLimitIterator += 1;
        }
        else
        {
            options.FPSLimitIterator = 0;
        }

        Application.targetFrameRate = options.FPSLimits[options.FPSLimitIterator];

        updateFPSLimitText();
    }

    public void lessFPSLimit()
    {
        if (options.FPSLimitIterator > 0)
        {
            options.FPSLimitIterator -= 1;
        }
        else
        {
            options.FPSLimitIterator = options.FPSLimits.GetLength(0) - 1;
        }

        Application.targetFrameRate = options.FPSLimits[options.FPSLimitIterator];
        
        updateFPSLimitText();
    }

    void updateFPSLimitText()
    {
        FPSLimit.text = (options.FPSLimits[options.FPSLimitIterator]).ToString();
    }

    public void moreSFX_Volume()
    {
        if (options.currentSFX_Volume <= 95)
        {
            options.currentSFX_Volume += 5;
            options.sfx.setVolume(options.currentSFX_Volume / 100.0f);

            sfxVolume.text = options.currentSFX_Volume.ToString(); //UpdateText
        }
        else
        {

        }
    }

    public void lessSFX_Volume()
    {
        if (options.currentSFX_Volume >= 5)
        {
            options.currentSFX_Volume -= 5;
            options.sfx.setVolume(options.currentSFX_Volume / 100.0f);

            sfxVolume.text = options.currentSFX_Volume.ToString(); //UpdateText
        }
        else
        {

        }
    }

    public void moreMusicVolume()
    {
        if (options.currentMusicVolume <= 95)
        {
            options.currentMusicVolume += 5;
            options.music.setVolume(options.currentMusicVolume / 100.0f);

            musicVolume.text = options.currentMusicVolume.ToString(); //UpdateText
        }
        else 
        {
        
        }
    }

    public void lessMusicVolume()
    {
        if (options.currentMusicVolume >= 5)
        {
            options.currentMusicVolume -= 5;
            options.music.setVolume(options.currentMusicVolume / 100.0f);

            musicVolume.text = options.currentMusicVolume.ToString(); //UpdateText
        }
        else
        {

        }
    }
    
    public void lessLanguage()
    {
        if (options.languageIterator > 0)
        {
            options.languageIterator -= 1;
        }
        else
        {
            options.languageIterator = options.languageMax;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[options.languageIterator];

        updateLanguageText();
    }

    public void moreLanguage()
    {
        if (options.languageIterator < options.languageMax)
        {
            options.languageIterator += 1;
        }
        else
        {
            options.languageIterator = 0;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[options.languageIterator];

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
            else
                test.Completed += (t) => language.text = test.Result;
        }
        else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]) //Español
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Castellano");

            if (test.IsDone)
            {
                language.text = test.Result;
            }
            else
                test.Completed += (t) => language.text = test.Result;
        }

        if (options.fullscreen)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (t) => fullScreen.text = test.Result;
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                fullScreen.text = test.Result;
            }
            else
                test.Completed += (t) => fullScreen.text = test.Result;
        }

        resolution.text = options.resolutionsString[options.currentResolutionIndex];

        if (options.vSyncOn)
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Yes");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }
            else
                test.Completed += (t) => VSync.text = test.Result;
        }
        else
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "No");

            if (test.IsDone)
            {
                VSync.text = test.Result;
            }
            else
                test.Completed += (t) => VSync.text = test.Result;
        }
    }

    public void SaveSettings()
    {
        GameManager.instance.optionsLoader.SaveSettings();
    }
}
