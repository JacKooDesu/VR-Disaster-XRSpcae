using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTorus : Stage
{
    public GameObject torus;
    public Transform handPosition;
    public GameObject Extinguisher;

    [Header("UI設定")]
    public ObjectSwitcher uiSwitcher;
    public GameObject progressImage;
    CoroutineUtility.Timer uiTimer;
    public MaterialChanger changer;

    public override async void OnBegin()
    {
        base.OnBegin();
        uiSwitcher.gameObject.SetActive(true);
        await System.Threading.Tasks.Task.Yield();
        changer.ChangeColor();
        uiSwitcher.Switch(0);
        progressImage.SetActive(true);
        uiTimer = new CoroutineUtility.Timer(3f, () => uiSwitcher.HideAll());

        torus.GetComponent<Outline>().enabled = true;
        var torusInteract = torus.GetComponent<InteracableObject>();
        torusInteract.enabled = true;
        torusInteract.onHoverEvent.AddListener(() => isFinish = true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        progressImage.SetActive(false);
        uiTimer.Stop(true);
        changer.BackOriginColor();

        torus.SetActive(false);
        // torus.GetComponent<Outline>().enabled = false;
        // var torusInteract = torus.GetComponent<InteracableObject>();
        // torusInteract.onHoverEvent.RemoveAllListeners();
        // torusInteract.enabled = false;
    }
}
