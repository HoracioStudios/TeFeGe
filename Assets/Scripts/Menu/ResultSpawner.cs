using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSpawner : MonoBehaviour
{
    public GameObject prefabResult;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance)
        {
            for (int i = 0; i < GameManager.instance.results.Count; i++)
            {
                GameObject result = Instantiate(prefabResult, gameObject.transform);

                GameManager.RoundResult r = GameManager.instance.results[i];

                Color col = Color.white;

                switch (r.result)
                {
                    case 1.0:
                        col = Color.green;
                        break;

                    case 0.5:
                        col = Color.yellow;
                        break;

                    case 0.0:
                        col = Color.red;
                        break;

                    default:
                        break;
                }

                result.transform.GetChild(0).GetComponent<Image>().color = col;
                result.transform.GetChild(1).GetComponent<Text>().text = (r.time * 45.0f).ToString() + " s";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
