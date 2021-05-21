using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class InputFieldDeselect : MonoBehaviour, IDeselectHandler
{
    [SerializeField] int type; //0 Mail, 1 User, 2 Pass

    [SerializeField] private GameObject errorGameObject;

    public void OnDeselect(BaseEventData eventData)
    {
        switch(type)
        {
            case 0: //Mail

                string email = this.GetComponent<InputField>().text;

                if (!Utility.isEmail(email))
                {
                    errorGameObject.SetActive(true);
                }

                break;

            case 1: //User

                string user = this.GetComponent<InputField>().text;

                if (!Utility.UsernameMinCharacters(user))
                {
                    errorGameObject.SetActive(true);

                    var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ValidUsernameLength");

                    if (test.IsDone)
                    {
                        errorGameObject.transform.GetChild(1).GetComponent<Text>().text = test.Result;
                    }
                    else
                        test.Completed += (test1) => Debug.Log(test.Result);

                    return;
                }
                else if (!Utility.isUsername(user))
                {
                    errorGameObject.SetActive(true);

                    var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ValidUsernameChars");

                    if (test.IsDone)
                    {
                        errorGameObject.transform.GetChild(1).GetComponent<Text>().text = test.Result;
                    }
                    else
                        test.Completed += (test1) => Debug.Log(test.Result);

                    return;
                }

                break;
            case 2: //Pass

                string pass = this.GetComponent<InputField>().text;

                if (!Utility.PasswordMinCharacters(pass))
                {
                    errorGameObject.SetActive(true);
                }

                break;
            default:
                break;
        }
    }
}
