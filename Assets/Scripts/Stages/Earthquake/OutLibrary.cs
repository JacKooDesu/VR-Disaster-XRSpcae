using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLibrary : Stage
{
    public override void OnBegin()
    {
        base.OnBegin();
        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.missionComplete);
        isFinish = true;
    }
}
