using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWindow : Stage
{
    public GlassController glassController;
    public int needBreak = 5;
    public GameObject[] handBreakers; // 拿在玩家手上的擊破器

    public override void OnBegin()
    {
        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.glassBreaker);

        glassController.ShowHint(true);
        glassController.EnableBreakers(true);

        foreach (var b in handBreakers)
            b.SetActive(true);
    }

    // public override void OnUpdate()
    // {
    // }

    public override void OnFinish()
    {
        base.OnFinish();

        foreach (var b in handBreakers)
            b.SetActive(false);
    }
}
