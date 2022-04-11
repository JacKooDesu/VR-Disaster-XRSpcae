using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRescueKit : Stage
{
    public List<KitItem> requireItems = new List<KitItem>();

    public override void OnBegin()
    {
        base.OnBegin();
        FindObjectOfType<HintCanvas>().SetHintText("整理急難救助包！", true);
        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.selectWhistle);

        GameHandler.Singleton.player.ShowKit();
    }

    public void TakeWhistle()
    {
        print("whistle");
        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.whistle);

        // StartCoroutine(
        //     GameHandler.Singleton.Counter(
        //         audio.whistle.length + 1,
        //         audio.whistle.length + 1,
        //         delegate
        //         {
        //             audio.PlaySound(audio.missionComplete);
        //         }
        //     )
        // );

        // StartCoroutine(
        //     GameHandler.Singleton.Counter(
        //         audio.whistle.length + 1 + audio.missionComplete.length + 1,
        //         audio.whistle.length + 1 + audio.missionComplete.length + 1,
        //         delegate
        //         {
        //             GameHandler.Singleton.StageFinish();
        //             ui.TurnOff();
        //         }
        //     )
        // );
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var item in requireItems)
        {
            if (!item.inPack)
                return;
        }

        isFinish = true;
    }

    public override void OnFinish()
    {
        base.OnFinish();
    }
}
