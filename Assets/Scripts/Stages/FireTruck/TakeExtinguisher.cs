using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeExtinguisher : Stage
{
    public Animator topExitAni;

    public InteracableObject fireExtinguisherBody;

    public MaterialChanger changer;

    public override void OnBegin()
    {
        base.OnBegin();
        changer.ChangeColor();
        GameHandler.Singleton.player.PathFinding(fireExtinguisherBody.transform.position);

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

        fireExtinguisherBody.onHoverEvent.AddListener(() => print("拿取滅火器"));
        fireExtinguisherBody.onHoverEvent.AddListener(() => isFinish = true);

        FindObjectOfType<HintCanvas>().SetHintText("照著箭頭引導指示拿取滅火器", true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        changer.BackOriginColor();
        GameHandler.Singleton.player.line.gameObject.SetActive(false);

        fireExtinguisherBody.onHoverEvent.RemoveAllListeners();
    }
}
