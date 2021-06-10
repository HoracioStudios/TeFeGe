using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProfileData : MonoBehaviour
{
    public Text nick, rating, wins, draws, losses;

    public void DoUpdate()
    {
        if(rating && wins && draws && losses && nick)
        {
            Debug.Log(GameManager.instance.ID);
            ServerMessage msg = ClientCommunication.GetInfo(GameManager.instance.ID);

            if (msg.code != 200)
            {
                GameManager.instance.ThrowErrorScreen(msg.code);
            }
            else
            {
                UserDataSmall m = (UserDataSmall)msg;

                nick.text = m.nick;
                rating.text = m.rating.ToString();
                wins.text = m.wins.ToString();
                draws.text = m.draws.ToString();
                losses.text = m.losses.ToString();
            }
        }
    }
}
