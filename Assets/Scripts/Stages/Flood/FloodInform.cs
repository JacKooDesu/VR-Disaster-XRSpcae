using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodInform : Stage
{
    public override void OnBegin()
    {
        base.OnBegin();

        GameHandler.Singleton.player.SetCanMove(false);
    }

    public override void OnFinish()
    {
        base.OnFinish();
        
        GameHandler.Singleton.player.SetCanMove(true);
    }
}
