using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFlashLight : Stage
{
    public GameObject pointLights;
    public UIQuickSetting ui;

    public override void OnBegin()
    {
        base.OnBegin();

        pointLights.SetActive(false);

        ui.gameObject.SetActive(true);
        ui.TurnOn();

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.getRescueKit);
    }

    public override void OnFinish()
    {
        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.StopAll();
    }
}
