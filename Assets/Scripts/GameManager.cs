using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using XInputDotNetPure; // Required in C#

public class GameManager : MonoBehaviour
{
    public bool loggedIn = false;

    public GameData gameData;

    public int totalRounds = 3;

    public int currentRound = 0;

    public List<RoundResult> results = new List<RoundResult>();

    //[HideInInspector]
    public bool isControllerMode = false;

    public int ID;

    public bool inQueue = false;

    private string nick = "";

    public bool isServer = false;

    public string version = "1.0.0";

    //get público, set privado
    static public GameManager instance { get; private set; }

    //public User player { get; set; }

    [HideInInspector]
    public RoundManager roundManager;

    public GameObject errorScreenPrefab;

    public OptionsLoader optionsLoader;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined; //Lock del raton

        // si es la primera vez que accedemos a la instancia del GameManager,
        // no existira, y la crearemos
        if (instance == null)
        {
            Application.runInBackground = true;
            // guardamos en la instancia el objeto creado
            // debemos guardar el componente ya que _instancia es del tipo GameManager
            instance = this;

            // hacemos que el objeto no se elimine al cambiar de escena
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Start()
    {
        if (!instance.optionsLoader)
        {
            instance.optionsLoader = instance.gameObject.AddComponent<OptionsLoader>();
            instance.optionsLoader.Init();
        }
    }
    
    public float vibrationLength = 0.1f;
    public float vibrationTimer = 0f;
    public bool vibrating = false;

    void Update()
    {   
        if (vibrating)
        {
            if (vibrationTimer < vibrationLength) vibrationTimer += Time.deltaTime;
            else
            {
                vibrating = false;
                vibrationTimer = 0;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }
        }

        //if (Input.GetKeyDown(KeyCode.M)) StartVibration();

        if (isControllerMode)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Fire") != 0 || Input.GetAxis("FireAbility") != 0)
            {
                isControllerMode = false;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal_Joy") != 0 || Input.GetAxis("Vertical_Joy") != 0 || Input.GetAxis("Aim_X") != 0 || Input.GetAxis("Aim_Y") != 0 || Input.GetAxis("Fire_Joy") != 0 || Input.GetAxis("FireAbility_Joy") != 0)
            {
                isControllerMode = true;
            }
        }
    }

    public void StartVibration(float vibrationStrength)
    {
        vibrating = true;
        GamePad.SetVibration(PlayerIndex.One, vibrationStrength, vibrationStrength);
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    public void SendResults()
    {
        results.Clear();
    }

    public void ThrowErrorScreen(int error, string message = "unknown error")
    {
        if (!errorScreenPrefab)
        {
            Debug.Log(error);
            return;
        }

        string stringName = "";

        switch (error)
        {
            case -2: //petición inválida por error de socket

                stringName = "CannotConnect";

                break;
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

        if (stringName != "")
        {

            var test = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", stringName);

            if (test.IsDone)
            {
                instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = test.Result;
            }
            else
                test.Completed += (test1) => instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = test.Result;
        }
        else
            instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = message;

        instance.transform.GetChild(2).gameObject.GetComponent<Text>().text = "ERROR";

        if(error > 0)
            instance.transform.GetChild(2).gameObject.GetComponent<Text>().text += "\"" + error + '"';
    }

    public void ThrowScreen(int error, string stringName)
    {
        if (!errorScreenPrefab)
        {
            Debug.Log(error);
            return;
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


        instance.transform.GetChild(2).gameObject.GetComponent<Text>().text = "";
        instance.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.black;
    }

    public void SetNick(string nick){ this.nick = nick; }
    public string GetNick() { return nick; } 


    //[Command]
    //public void bbbb()
    //{
    //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //}


    // Constructor
    // Lo ocultamos el constructor para no poder crear nuevos objetos "sin control"
    protected GameManager() { }

    private void OnApplicationQuit()
    {    
        if(!isServer)
        { 
            GamePad.SetVibration(PlayerIndex.One, 0, 0);

            try
            {
                ClientCommunication.LogOut();
            }
            catch (System.Exception)
            {
                GameManager.instance.ThrowErrorScreen(-2);
            }
        }
    }
}
