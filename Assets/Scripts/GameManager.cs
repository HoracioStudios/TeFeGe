using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : MonoBehaviour
{

    //[HideInInspector]
    public bool isControllerMode = false;

    public RoundManager roundManager = null;

    //get público, set privado
    static public GameManager instance { get; private set; }

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

    public void StartGame(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void StartGame(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
