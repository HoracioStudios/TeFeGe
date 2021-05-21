using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveButtons : MonoBehaviour
{
    [SerializeField]
    private GameObject LogInButtons;

    [SerializeField]
    private GameObject CreateAccountButtons;

    [SerializeField]
    private GameObject[] errorGameObjects;

    public void setCreateAccountButtonsActive()
    {
        LogInButtons.SetActive(false);

        for (int i = 0; i < errorGameObjects.Length; i++)
        {
            errorGameObjects[i].SetActive(false);
        }

        CreateAccountButtons.SetActive(true);
    }
    
    public void setLogInButtonsActive()
    {
        CreateAccountButtons.SetActive(false);
        
        for (int i = 0; i < errorGameObjects.Length; i++)
        {
            errorGameObjects[i].SetActive(false);
        }

        LogInButtons.SetActive(true);
    }
}
