using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class CreateAccountScript : MonoBehaviour
{
    [SerializeField] private InputField usernameInputField = default;
    [SerializeField] private InputField passwordInputField = default;
    [SerializeField] private InputField emailInputField = default;
    
    [SerializeField] private GameObject emailError;
    [SerializeField] private GameObject nickError;
    [SerializeField] private GameObject passError;

    [SerializeField] private GameObject createAccountError;

    public void CreateAccount()
    {
        createAccountError.SetActive(false);

        string user = usernameInputField.text;
        string email = emailInputField.text;
        string pass = passwordInputField.text;

        //Comprobación de que el introducido es un mail valido
        if (Utility.isEmail(email))
        {
            if (Utility.UsernameMinCharacters(user))
            {
                if (Utility.isUsername(user))
                {
                    if (Utility.PasswordMinCharacters(pass))
                    {
                        //ENCRIPTACION

                        string userEnc = Utility.sha256FromString(user);
                        string emailEnc = Utility.sha256FromString(email);
                        string passEnc = Utility.sha256FromString(pass);

                        //ENVIO DE PETICION DE CREACION DE CUENTA

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
        else //Email no valido
        {
            emailError.SetActive(true);
        }
    }

    public void CreateAccountError(int error)
    {
        switch (error)
        {
            case 400: //petición inválida por falta de algún dato obligatorio
                createAccountError.SetActive(true);

                var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "CreateAccountMissingData");

                if (test.IsDone)
                {
                    createAccountError.GetComponent<Text>().text = test.Result;
                }
                else
                    test.Completed += (test1) => createAccountError.GetComponent<Text>().text = test.Result;

                break;
            case 502: //la base de datos no acepta conexión
                createAccountError.SetActive(true);

                var testB = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "CreateAccountConnection");

                if (testB.IsDone)
                {
                    createAccountError.GetComponent<Text>().text = testB.Result;
                }
                else
                    testB.Completed += (test1) => createAccountError.GetComponent<Text>().text = testB.Result;

                break;
            default:
                break;
        }
    }
}
