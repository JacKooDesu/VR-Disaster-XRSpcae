using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRescueKit : Stage
{
    public UIQuickSetting ui;

    public override void OnBegin()
    {
        base.OnBegin();
        // ui.TurnOn();
        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.selectWhistle);
    }

    public void TakeWhistle()
    {
        print("whistle");
        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.whistle);

        StartCoroutine(
            GameHandler.Singleton.Counter(
                audio.whistle.length + 1,
                audio.whistle.length + 1,
                delegate
                {
                    audio.PlaySound(audio.missionComplete);
                }
            )
        );

        StartCoroutine(
            GameHandler.Singleton.Counter(
                audio.whistle.length + 1 + audio.missionComplete.length + 1,
                audio.whistle.length + 1 + audio.missionComplete.length + 1,
                delegate
                {
                    GameHandler.Singleton.StageFinish();
                    ui.TurnOff();
                }
            )
        );
    }

    public override void OnFinish()
    {
        base.OnFinish();
    }
}
