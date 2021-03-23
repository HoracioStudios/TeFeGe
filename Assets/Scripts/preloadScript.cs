using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class preloadScript : MonoBehaviour
{
    public string firstScene = "";
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(Application.CanStreamedLevelBeLoaded(firstScene))
            SceneManager.LoadScene(firstScene);
    }
}
