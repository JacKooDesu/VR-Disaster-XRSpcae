using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineUtility;

public class TakeBreaker : Stage
{
    public InteracableObject[] breakers;    // 掛在牆上的

    public MaterialChanger changer;

    public GameObject informUi;
    Timer uiTimer;

    public override void OnBegin()
    {
        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;
        audio.StopCurrent();
        audio.PlaySound(audio.takeBreaker);

        foreach (var b in breakers)
        {
            b.onHoverEvent.AddListener(() => isFinish = true);
            b.interactable = true;
        }

        changer.ChangeColor();

        uiTimer = new Timer(
            5f,
            () => informUi.SetActive(true),
            (f) => { },
            () => informUi.SetActive(false));
    }

    public override void OnFinish()
    {
        base.OnFinish();
        foreach (var b in breakers)
        {
            b.interactable = false;
        }

        changer.BackOriginColor();

        uiTimer.Stop(true);
    }
}
