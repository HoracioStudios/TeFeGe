using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackerUAJ;

public class Telemetria : MonoBehaviour
{

    public int shots = 0;
    public float dmg = 0;

    static EndSessionEvent endEvent;

    public static Telemetria instance { get; private set; }

    private void Awake()
    {
        // si es la primera vez que accedemos a la instancia del GameManager,
        // no existira, y la crearemos
        if (instance == null)
        {
            // guardamos en la instancia el objeto creado
            // debemos guardar el componente ya que _instancia es del tipo GameManager
            instance = this;
            InitTelemetria();

            // hacemos que el objeto no se elimine al cambiar de escena
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Se llama cuando se inicia sesion
    public void InitTelemetria()
    {
        //Telemetria inicio
        Tracker.GetInstance().Init(GameManager.instance.playerID);
        Tracker.GetInstance().AddPersistance(new TextPersistance("Telemetria/trackerJSON.json"), TraceFormats.JSON);
        Tracker.GetInstance().AddPersistance(new TextPersistance("Telemetria/trackerXML.xml"), TraceFormats.XML);
        //Tracker.GetInstance().AddPersistance(new ServerPersistance("http://localhost:8080/tracker"), TraceFormats.JSON);
        endEvent = Tracker.GetInstance().getEndSessionEvent();
    }

    public void CharacterSelectionEvent()
    {
        int character = ((ExtendedNetworkManager)Mirror.NetworkManager.singleton).playerSelection;
        TrackerEvent e = Tracker.GetInstance().getCharacterSelectorEvent().SetCharacterSelected(GameManager.instance.characterNames[character]);
        Tracker.GetInstance().SendEvent(e);
    }

    public void EndGameEvent()
    {
        int character = ((ExtendedNetworkManager)Mirror.NetworkManager.singleton).playerSelection;
        EndGameEvent e = Tracker.GetInstance().getEndGame()
            .SetPlayerName(GameManager.instance.characterNames[character])
            .SetShots(shots)
            .SetDamage(dmg);
        for (int i = 0; i < 3; i++)
        {
            e.SetResultRound(GameManager.instance.results[i].result, i)
                .SetTimeRound(GameManager.instance.results[i].time, i);
        }

        Tracker.GetInstance().TrackEvent(e);
        ResetValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Tracker.GetInstance().TrackEvent(endEvent);
            Tracker.GetInstance().Flush();
        }
    }

    private void OnApplicationQuit()
    {        
        Tracker.GetInstance().SendEvent(endEvent);
        Tracker.GetInstance().End();
    }

    private void ResetValues()
    {
        shots = 0;
        dmg = 0;
    }
}
