using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class InputFieldSelect : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject errorGameObject;
    [SerializeField] private GameObject AccountErrorGameObject;

    public void OnSelect(BaseEventData eventData)
    {
        errorGameObject.SetActive(false);
        AccountErrorGameObject.SetActive(false);
    }
}
