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

    [SerializeField] private GameObject canvasToClose;

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
                    Debug.Log("Intento de inicio de sesion");
                    try
                    {
                        ServerMessage msg = ClientCommunication.LogIn(passEnc, user);
                        if (msg.code != 200)
                        {
                            Debug.Log("Inicio de sesion error");
                            LogInError(msg.code);
                        }
                        else
                        {
                            Login m = (Login)msg;
                            Debug.Log("Inicio de sesion correcto");
                            GameManager.instance.loggedIn = true;
                            GameManager.instance.ID = m.id;
                            GameManager.instance.SetNick(user);
                            GameManager.instance.LoadScene("MainMenu");
                        }
                    }
                    catch (System.Exception)
                    {
                        GameManager.instance.ThrowErrorScreen(-2);
                    }
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
