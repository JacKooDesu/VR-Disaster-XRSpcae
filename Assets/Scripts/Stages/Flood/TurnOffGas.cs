using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnOffGas : Stage
{
    public InteracableObject gasSwitch;
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
        gasSwitch.interactable = false;
        fire.SetActive(false);
    }
}
