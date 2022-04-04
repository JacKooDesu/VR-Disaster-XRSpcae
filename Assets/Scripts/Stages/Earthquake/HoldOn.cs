using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRSpace.Platform.InputDevice;
using XRSpace.Platform.VRcore;

public class HoldOn : Stage
{
    public Transform tableLower;

    public MaterialChanger changer;

    public override void OnBegin()
    {
        base.OnBegin();
        GameHandler.Singleton.player.SetCanMove(false);
        XRActionGestureManager.ActionDetectedEvent += CheckHandHoldingEvent;
        // UI.TurnOn();
        // GameHandler.Singleton.BlurCamera(true);
        // StartCoroutine(GameHandler.Singleton.Counter(
        //     hintDisplayTime,
        //     delegate
        //     {
        //         UI.TurnOff();
        //         GameHandler.Singleton.BlurCamera(false);
        //     }));
        changer.ChangeColor();
    }

    public override void OnUpdate()
    {
        // if (GameHandler.Singleton.cam.transform.position.y > tableTop.position.y)
        // {
        //     if (!waringHUD.gameObject.activeInHierarchy)
        //     {
        //         waringHUD.gameObject.SetActive(true);
        //     }
        //     waringHUD.TurnOn();
        //     GameHandler.Singleton.BlurCamera(true);
        // }
        // else
        // {

        //     waringHUD.TurnOff();
        //     GameHandler.Singleton.BlurCamera(false);
        // }
    }

    void CheckHandHoldingEvent(XRDeviceType deviceType, XRActionGesture actionGesture)
    {
        if (actionGesture == XRActionGesture.Grab_Outward)
        {
            var hand = deviceType == XRDeviceType.HANDLER_LEFT ? GameHandler.Singleton.player.leftHandler : GameHandler.Singleton.player.rightHandler;
            if ((hand.position - tableLower.position).magnitude <= .5f)
            {
                isFinish = true;
            }
        }
    }


    public override void OnFinish()
    {
        base.OnFinish();
        XRActionGestureManager.ActionDetectedEvent -= CheckHandHoldingEvent;

        // UI.TurnOff();

        // 2021.03.11 
        // UI.transform.parent.gameObject.SetActive(false);
        // GameHandler.Singleton.cam.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = false;

        // tweener.MoveNextPoint();

        changer.BackOriginColor();
    }
}
