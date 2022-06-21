using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineUtility;

public class FindFlashLight : Stage
{
    public GameObject pointLights;

    public GameObject flashlight;   // 手電筒

    public override void OnBegin()
    {
        base.OnBegin();

        pointLights.SetActive(false);

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.getRescueKit);

        var timer = new Timer(a.getRescueKit.length, () => flashlight.SetActive(true));
    }

    public override void OnFinish()
    {
        base.OnFinish();
        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.StopAll();
    }
}
