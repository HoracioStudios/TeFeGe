using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActiveButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject LogInButtons;

    [SerializeField]
    private GameObject CreateAccountButtons;

    [SerializeField]
    private GameObject[] errorGameObjects;

    [SerializeField]
    private GameObject[] inputFields;

    public void setCreateAccountButtonsActive()
    {
        LogInButtons.SetActive(false);

        for (int i = 0; i < errorGameObjects.Length; i++)
        {
            errorGameObjects[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].GetComponent<InputField>().text = "";
        }

        CreateAccountButtons.SetActive(true);
    }
    
    public void setLogInButtonsActive()
    {
        CreateAccountButtons.SetActive(false);
        
        for (int i = 0; i < errorGameObjects.Length; i++)
        {
            errorGameObjects[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].GetComponent<InputField>().text = "";
        }

        LogInButtons.SetActive(true);
    }
}
