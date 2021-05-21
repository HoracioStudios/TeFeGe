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
        logInError.SetActive(false);

        string user = usernameInputField.text;
        string pass = passwordInputField.text;

        if (Utility.UsernameMinCharacters(user))
        {
            if (Utility.isUsername(user))
            {
                if (Utility.PasswordMinCharacters(pass))
                {
                    //ENCRIPTACION

                    string userEnc = Utility.sha256FromString(user);
                    string passEnc = Utility.sha256FromString(pass);

                    //ENVIO DE PETICION DE LOG IN

                    ///////////////
                    ///


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
                    nickError.transform.GetChild(1).GetComponent<Text>().text = test.Result;
                }
                else
                    test.Completed += (test1) => Debug.Log(test.Result);
            }
        }
        else //Nick inferior a caracteres minimos
        {
            nickError.SetActive(true);

            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ValidUsernameLength");

            if (test.IsDone)
            {
                nickError.transform.GetChild(1).GetComponent<Text>().text = test.Result;
            }
            else
                test.Completed += (test1) => Debug.Log(test.Result);
        }
    }

    public void LogInError(int error)
    {
        switch (error)
        {
            case 400: //petición inválida por falta de algún dato obligatorio
                logInError.SetActive(true);

                var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "CreateAccountMissingData");

                if (test.IsDone)
                {
                    logInError.transform.GetChild(1).GetComponent<Text>().text = test.Result;
                }
                else
                    test.Completed += (test1) => Debug.Log(test.Result);

                break;
            case 404: //no se ha encontrado un usuario con esos credenciales
                logInError.SetActive(true);

                var testA = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "LoginErrorNotFound");

                if (testA.IsDone)
                {
                    logInError.transform.GetChild(1).GetComponent<Text>().text = testA.Result;
                }
                else
                    testA.Completed += (test1) => Debug.Log(testA.Result);

                break;
            case 502: //la base de datos no acepta conexión
                logInError.SetActive(true);

                var testB = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "CreateAccountConnection");

                if (testB.IsDone)
                {
                    logInError.transform.GetChild(1).GetComponent<Text>().text = testB.Result;
                }
                else
                    testB.Completed += (test1) => Debug.Log(testB.Result);

                break;
            default:
                break;
        }
    }
}
