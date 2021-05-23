using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class EmptyTextPlaceholder : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private string key;

    // Update is called once per frame
    void Update()
    {
        if (text.text == "") //Si está vacío, poner texto
        {
            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", key);

            if (test.IsDone)
            {
                this.GetComponent<Text>().text = test.Result;
            }
            else
                test.Completed += (test1) => this.GetComponent<Text>().text = test.Result;
        }
        else
        {
            this.GetComponent<Text>().text = "";
        }
    }
}
