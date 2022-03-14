using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTable : Stage
{
    public override void OnBegin()
    {
        base.OnBegin();

        // UI.TurnOn();

        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.goUnderTable);
        audio.currentPlayingSound = null;

        var tp = FindStageObject<TeleportPoint>();
        tp.onTeleportAction.AddListener(() =>
        {
            isFinish = true;
            Destroy(tp.gameObject);
        });
    }
}
