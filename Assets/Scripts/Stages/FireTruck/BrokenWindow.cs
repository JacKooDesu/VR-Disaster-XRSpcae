using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWindow : Stage
{
    public GlassController glassController;
    public int needBreak = 5;


    public override void OnBegin()
    {
        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.glassBreaker);

        glassController.ShowHint(true);
        glassController.EnableBreakers(true);
    }

    // public override void OnUpdate()
    // {
    // }

    public override void OnFinish()
    {
        base.OnFinish();
    }
}
