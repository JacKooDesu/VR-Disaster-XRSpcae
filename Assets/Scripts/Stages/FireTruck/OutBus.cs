using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutBus : Stage
{
    public override void OnBegin()
    {
        base.OnBegin();
        isFinish = true;
    }

    public override void OnFinish()
    {
        base.OnFinish();
        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.finish);
    }
}
