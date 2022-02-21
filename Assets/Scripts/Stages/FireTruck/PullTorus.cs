using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTorus : Stage
{
    public GameObject torus;
    public UIQuickSetting ui;
    public Transform handPosition;
    public GameObject Extinguisher;
    public override void OnBegin()
    {
        base.OnBegin();
        ui.TurnOn();
        StartCoroutine(GameHandler.Singleton.Counter(3f, 3f, delegate { ui.TurnOff(); }));
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
        ui.TurnOff();
    }
}
