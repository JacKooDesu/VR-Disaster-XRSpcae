using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeExtinguisher : Stage
{
    public Animator topExitAni;

    public FireExtinguisher fireExtinguisher;

    public override void OnBegin()
    {
        base.OnBegin();
        topExitAni.enabled = true;
        GameHandler.Singleton.player.SetCanMove(true);

        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;

        audio.PlayAudio(audio.fireSound, true, stageObjects[0].obj.transform);
        audio.GetSpeakerAudioSource(audio.fireSound).volume = .5f;

        audio.PlaySound(audio.squatDown);
        audio.currentPlayingSound = null;
        StartCoroutine(GameHandler.Singleton.Counter(
            audio.squatDown.length,
            delegate
            {
                audio.PlaySound(audio.extinguisherTutorial);
                audio.currentPlayingSound = null;
            }
        ));

        audio.PlayAudio(audio.bgm2, true, GameHandler.Singleton.player.transform).volume = .1f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        base.OnFinish();
    }
}
