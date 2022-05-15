using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRSpace.Platform.InputDevice;

public class SprayFire : Stage
{
    public CustomControllerBehaviour controller;
    public ParticleSystem powder;
    [Header("UI設定")]
    public ObjectSwitcher uiSwitcher;
    public GameObject progressImage;
    CoroutineUtility.Timer uiTimer;

    // private variable
    JacDev.Audio.FireTruck audioHandler;
    public override void OnBegin()
    {
        base.OnBegin();

        audioHandler = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;

        uiSwitcher.Switch(3);
        progressImage.SetActive(true);
        uiTimer = new CoroutineUtility.Timer(3f, () => uiSwitcher.HideAll());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        bool isPressing = XRInputManager.Instance.Button((XRDeviceType)controller.Device, XRControllerButton.Trigger);
        if (isPressing && !powder.isPlaying)
        {
            powder.GetComponent<ParticleSystem>().Play();

            audioHandler.PlayAudio(audioHandler.extinguisher, false, transform);
        }
        else
        {
            powder.Stop();
            if (GetComponentInChildren<AudioSource>())
                Destroy(GetComponentInChildren<AudioSource>().gameObject);
        }
    }

    public override void OnFinish()
    {
        base.OnFinish();

        progressImage.SetActive(false);
        uiTimer.Stop(true);

        uiSwitcher.gameObject.SetActive(false);

        GameHandler.Singleton.player.SetCanMove(true);
    }
}
