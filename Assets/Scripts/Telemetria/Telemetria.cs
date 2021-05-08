using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrackerUAJ;

public class Telemetria : MonoBehaviour
{
    public void InitTelemetria()
    {
        //Telemetria inicio
        Tracker.GetInstance().Init(GameManager.instance.playerID);
        Tracker.GetInstance().AddPersistance(new TextPersistance("tracker.json"), TraceFormats.JSON);
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
            .SetShots(GameManager.instance.shots)
            .SetDamage(GameManager.instance.dmg);

        for (int i = 0; i < 3; i++)
        {
            e.SetResultRound(GameManager.instance.results[i].result, i)
                .SetTimeRound(GameManager.instance.results[i].time, i);
        }

        Tracker.GetInstance().TrackEvent(e);
        Tracker.GetInstance().End();
    }
}
