using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InformationMenu : MonoBehaviour
{
    public void goToTwitter()
    {
        Application.OpenURL("https://twitter.com/TeFeGe_game");
    }

    public void goToMail()
    {
        Application.OpenURL("mailto:tefege.game@gmail.com");
    }

    public void shareTwitter()
    {
        Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL("Jose porro + PPP = <3"));
    }
}
