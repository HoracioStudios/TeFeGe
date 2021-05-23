using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LogInScript : MonoBehaviour
{
    [SerializeField] private InputField usernameInputField = default;
    [SerializeField] private InputField passwordInputField = default;

    [SerializeField] private GameObject nickError;
    [SerializeField] private GameObject passError;

    [SerializeField] private GameObject logInError;

    public void LogIn()
    {
        //logInError.SetActive(false);

        string user = usernameInputField.text;
        string pass = passwordInputField.text;

        if (Utility.UsernameMinCharacters(user))
        {
            if (Utility.isUsername(user))
            {
                if (Utility.PasswordMinCharacters(pass))
                {
                    //ENCRIPTACION

                    //string userEnc = Utility.sha256FromString(user);
                    string passEnc = Utility.sha256FromString(pass);

                    //ENVIO DE PETICION DE LOG IN

                    Message m = ClientCommunication.LogIn(pass, user, "");
                    if (m.code != 200) LogInError(m.code);

                }
                else //Contraseña no válida
                {
                    passError.SetActive(true);
                }
            }
            else //Usuario no valido
            {
                nickError.SetActive(true);

                var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ValidUsernameChars");

                if (test.IsDone)
                {
                    nickError.GetComponent<Text>().text = test.Result;
                }
                else
                    test.Completed += (test1) => nickError.GetComponent<Text>().text = test.Result;
            }
        }
        else //Nick inferior a caracteres minimos
        {
            nickError.SetActive(true);

            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ValidUsernameLength");

            if (test.IsDone)
            {
                nickError.GetComponent<Text>().text = test.Result;
            }
            else
                test.Completed += (test1) => nickError.GetComponent<Text>().text = test.Result;
        }
    }

    public void LogInError(int error)
    {
        if (error == 404)
        {
            logInError.SetActive(true);

            var testA = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "LoginErrorNotFound");

            if (testA.IsDone)
            {
                logInError.GetComponent<Text>().text = testA.Result;
            }
            else
                testA.Completed += (test1) => logInError.GetComponent<Text>().text = testA.Result;
        }
        else
        {
            GameManager.instance.ThrowErrorScreen(error);
        }
    }
}
