using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalGateSide : Stage
{
    public GameObject spotlight;
    public Transform sideTarget;
    public Transform sideObject;

    public override void OnBegin()
    {
        spotlight.SetActive(true);
        sideTarget.gameObject.SetActive(true);

        foreach (Transform t in sideObject)
        {
            t.GetComponent<Collider>().enabled = true;
        }

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.instalGateSide);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (Transform t in sideObject)
        {
            if (!t.GetComponent<GateSide>().isSafe)
                return;
        }

        GameHandler.Singleton.StageFinish();
    }

    public override void OnFinish()
    {
        sideTarget.gameObject.SetActive(false);

        foreach (Transform t in sideObject)
        {
            t.GetComponent<Collider>().enabled = false;
        }
    }
}
