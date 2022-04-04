using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTable : Stage
{
    public MaterialChanger tableHint;
    public override void OnBegin()
    {
        base.OnBegin();

        tableHint.ChangeColor();

        // UI.TurnOn();

        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.goUnderTable);
        audio.currentPlayingSound = null;

        FindObjectOfType<HintCanvas>().SetHintText("找到桌椅避難掩護！", true);
        var tp = FindStageObject<TeleportPoint>();
        tp.onTeleportAction.AddListener(() =>
        {
            isFinish = true;
            Destroy(tp.gameObject);
        });
    }

    public override void OnFinish()
    {
        base.OnFinish();
        tableHint.BackOriginColor();
    }
}
