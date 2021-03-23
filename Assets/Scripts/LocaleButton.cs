using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class LocaleButton : MonoBehaviour
{
    private int selected = 0;
    private int totalLocales = 0;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        totalLocales = LocalizationSettings.AvailableLocales.Locales.Count;
        for (int i = 0; i < totalLocales; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;        
        }
    }

    // Start is called before the first frame update
    public void changeLocale()
    {

        Debug.Log(selected);

        if (selected < totalLocales - 1)
        {
            selected++;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selected];
        }
        else
        {
            selected = 0;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selected];
        }
    }
}
