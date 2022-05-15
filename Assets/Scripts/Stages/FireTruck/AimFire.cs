using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFire : Stage
{
    public Transform firespot;
    public CustomControllerBehaviour controller;
    [Header("UI設定")]
    public ObjectSwitcher uiSwitcher;
    public GameObject progressImage;
    CoroutineUtility.Timer uiTimer;

    public override void OnBegin()
    {
        base.OnBegin();

        GameHandler.Singleton.player.SetCanMove(false);

        uiSwitcher.Switch(1);
        progressImage.SetActive(true);
        uiTimer = new CoroutineUtility.Timer(3f, () => uiSwitcher.HideAll());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Transform origin = controller.transform;

        RaycastHit hit;
        if (origin != null)
        {
            Ray ray = new Ray(origin.position, origin.forward);
            if (Physics.Raycast(ray, out hit, 10f))
            {
                print(hit.transform.name);
                if (hit.transform == firespot)
                {
                    isFinish = true;
                }
            }
        }
    }

    public override void OnFinish()
    {
        base.OnFinish();

        progressImage.SetActive(false);
        uiTimer.Stop(true);
    }
}
