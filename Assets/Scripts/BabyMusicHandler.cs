using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyMusicHandler : MusicHandler
{
    public normalShoot shoot;
        
    void Update()
    {
        emitter.SetParameter("Bad Baby Bullets", shoot.actualBullets);
    }
}
