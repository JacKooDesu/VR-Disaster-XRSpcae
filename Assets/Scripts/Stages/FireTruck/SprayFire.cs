using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : Stage
{
    [Header("UI設定")]
    public ObjectSwitcher uiSwitcher;
    public GameObject progressImage;
    CoroutineUtility.Timer uiTimer;

    public override void OnBegin()
    {
        base.OnBegin();

        uiSwitcher.Switch(3);
        progressImage.SetActive(true);
        uiTimer = new CoroutineUtility.Timer(3f, () => uiSwitcher.HideAll());
    }

    public override void OnFinish()
    {
        base.OnFinish();

        progressImage.SetActive(false);
        uiTimer.Stop(true);
    }
}
