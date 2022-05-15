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

    public override async void OnBegin()
    {
        base.OnBegin();
        uiSwitcher.gameObject.SetActive(true);
        await System.Threading.Tasks.Task.Yield();
        uiSwitcher.Switch(0);
        progressImage.SetActive(true);
        uiTimer = new CoroutineUtility.Timer(3f, () => uiSwitcher.HideAll());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void ClickTorus()
    {
        StartCoroutine(WaitTorusAnimation());
    }

    IEnumerator WaitTorusAnimation()
    {
        torus.GetComponent<Animator>().enabled = true;
        while (torus.GetComponentInChildren<MeshRenderer>().enabled)
        {
            yield return null;
        }
        Extinguisher.transform.SetParent(handPosition);
        iTween.MoveTo(Extinguisher, handPosition.position, .5f);
        //iTween.RotateTo(Extinguisher, Vector3.zero, .5f);
        isFinish = true;
    }

    public override void OnFinish()
    {
        base.OnFinish();
        progressImage.SetActive(false);
        uiTimer.Stop(true);
    }
}
