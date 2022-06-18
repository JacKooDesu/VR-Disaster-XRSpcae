using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnOffPower : Stage
{
    public Transform electronicBoxDoor;
    public InteracableObject doorInteract;
    public InteracableObject switchInteract;

    public override void OnBegin()
    {
        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.turnOffSwitch);

        doorInteract.interactable = true;
        doorInteract.onHoverEvent.AddListener(
            () =>
            {
                switchInteract.interactable = true;
                doorInteract.interactable = false;
                electronicBoxDoor.DORotate(Vector3.down * 180, 1f,RotateMode.WorldAxisAdd);
            }
        );

        switchInteract.interactable = false;
        switchInteract.onHoverEvent.AddListener(
            () =>
            {
                a.PlayAudio(a.switchSound, false, switchInteract.transform);
                switchInteract.interactable = false;
                isFinish = true;
            }
        );
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
