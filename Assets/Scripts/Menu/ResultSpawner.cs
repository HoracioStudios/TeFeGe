using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class ResultSpawner : MonoBehaviour
{
    public GameObject prefabResult;

    public Image miniaturaSprite;
    public Text miniaturaTxt;

    public Text resultTxt;

    public Sprite[] playerSprites;
    public string[] playerNames;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance)
        {
            float finale = 0;

            for (int i = 0; i < GameManager.instance.results.Count; i++)
            {
                GameObject result = Instantiate(prefabResult, gameObject.transform);

                GameManager.RoundResult r = GameManager.instance.results[i];

                finale += r.result;

                Color col = Color.white;

                switch (r.result)
                {
                    case 1.0f:
                        col = Color.green;
                        break;

                    case 0.5f:
                        col = Color.yellow;
                        break;

                    case 0.0f:
                        col = Color.red;
                        break;

                    default:
                        break;
                }

                result.transform.GetChild(0).GetComponent<Image>().color = col;

                float t = (float)Mathf.Round(r.time * 45.0f * 1000f) / 1000f;
                result.transform.GetChild(1).GetComponent<Text>().text = (t).ToString() + " s";
            }

            if (NetworkManager.singleton)
            {
                int p = ((ExtendedNetworkManager)(NetworkManager.singleton)).playerSelection;

                if (p < playerSprites.Length && p < playerNames.Length)
                {
                    miniaturaSprite.sprite = playerSprites[p];
                    miniaturaTxt.text = playerNames[p];
                }
            }

            finale = finale / (float)GameManager.instance.results.Count;

            UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<string> txt;

            if (finale < 0.5f) txt = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Defeat");
            else if (finale > 0.5f) txt = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Victory");
            else txt = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "Draw");

            if (txt.IsDone)
                resultTxt.text = txt.Result;
            else
                txt.Completed += (t) => resultTxt.text = txt.Result;

            GameManager.instance.SendResults();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
