using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TurnOffGas : Stage
{
    public InteracableObject gasSwitch;
    public Transform switchModel;
    public GameObject fire;

    public override void OnBegin()
    {
        base.OnBegin();

        GameHandler.Singleton.player.PathFinding(transform.position);

        gasSwitch.interactable = true;
        gasSwitch.onHoverEvent.AddListener(() => isFinish = true);

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        AudioSource boil = a.PlaySound(a.boilWater);
        StartCoroutine(GameHandler.Singleton.Counter(5, delegate
       {
           boil.volume = .4f;
           a.PlaySound(a.turnOffGas);
       }));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        gasSwitch.interactable = false;
        fire.SetActive(false);
        switchModel.DORotate(Vector3.up * 90, .5f, RotateMode.LocalAxisAdd);
        GameHandler.Singleton.player.line.gameObject.SetActive(false);
    }
}
