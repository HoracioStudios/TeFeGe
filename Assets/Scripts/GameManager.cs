﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Mirror;
using System;

public enum CharacterNames { BADBABY, BOB, CAMOMILA, CHUERK, MANOLO }


public class GameManager : MonoBehaviour
{
    [Serializable]
    public class RoundResult
    {
        public RoundResult(float res, float t) { result = res; time = t; }

        public float result;
        public float time;
    }

    public string[] characterNames = new string[] { "Bad Baby", "Bob Ojocojo", "Camomila Sestima", "Chuerk Chuerk", "Manolo MCfly" };

    public int playerID = 1;
    public int idGame = 0;

    public int totalRounds = 3;

    public int currentRound = 0;

    public List<RoundResult> results = new List<RoundResult>();

    //[HideInInspector]
    public bool isControllerMode = false;

    //get público, set privado
    static public GameManager instance { get; private set; }

    public User player { get; set; }

    [HideInInspector]
    public RoundManager roundManager;

    public GameObject errorScreenPrefab;

    private void Awake()
    {
        // si es la primera vez que accedemos a la instancia del GameManager,
        // no existira, y la crearemos
        if (instance == null)
        {
            // guardamos en la instancia el objeto creado
            // debemos guardar el componente ya que _instancia es del tipo GameManager
            instance = this;

            // hacemos que el objeto no se elimine al cambiar de escena
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        //CursorController.instance.ActivateYellowCursor();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M)) ThrowErrorScreen(-1);

        if (isControllerMode)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Fire") != 0)
            {
                isControllerMode = false;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal_Joy") != 0 || Input.GetAxis("Vertical_Joy") != 0 || Input.GetAxis("Aim_X") != 0 || Input.GetAxis("Aim_Y") != 0 || Input.GetAxis("Fire_Joy") != 0)
            {
                isControllerMode = true;
            }
        }
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void SendResults()
    {
        results.Clear();
    }

    public void ThrowErrorScreen(int error)
    {
        if (!errorScreenPrefab)
        {
            Debug.Log(error);
            return;
        }

        string stringName = "";

        switch (error)
        {
            case -1: //petición inválida por error de socket

                stringName = "SocketError";

                break;
            case 400: //petición inválida por falta de algún dato obligatorio

                stringName = "CreateAccountMissingData";

                break;
            case 502: //la base de datos no acepta conexión

                stringName = "CreateAccountConnection";

                break;
            default:
                break;
        }

        GameObject instance = Instantiate(errorScreenPrefab);

        instance.GetComponent<Canvas>().worldCamera = Camera.main;

        var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", stringName);

        if (test.IsDone)
        {
            instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = test.Result;
        }
        else
            test.Completed += (test1) => instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = test.Result;


        instance.transform.GetChild(2).gameObject.GetComponent<Text>().text = "ERROR \"" + error + '"';
    }


    //[Command]
    //public void bbbb()
    //{
    //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //}


    // Constructor
    // Lo ocultamos el constructor para no poder crear nuevos objetos "sin control"
    protected GameManager() { }
}
