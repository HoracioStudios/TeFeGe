using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public class RoundResult
    {
        public RoundResult(float res, float t) { result = res; time = t; }

        public float result;
        public float time;
    }

    public int totalRounds = 3;

    public int currentRound = 0;

    public List<RoundResult> results = new List<RoundResult>();

    //[HideInInspector]
    public bool isControllerMode = false;

    //get público, set privado
    static public GameManager instance { get; private set; }

    [HideInInspector]
    public RoundManager roundManager;

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


    //[Command]
    //public void bbbb()
    //{
    //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //}


    // Constructor
    // Lo ocultamos el constructor para no poder crear nuevos objetos "sin control"
    protected GameManager() { }
}
