using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalGateSide : Stage
{
    public GameObject spotlight;
    public Transform targetParent;
    public Transform objParent;

    public override void OnBegin()
    {
        spotlight.SetActive(true);
        targetParent.gameObject.SetActive(true);

        foreach (Transform t in objParent)
        {
            var interact = t.GetComponent<GateSide>();
            interact.Interactable = true;
        }

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.instalGateSide);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (Transform t in objParent)
        {
            if (!t.GetComponent<GateSide>().hasInstalled)
                return;
        }
        isFinish=true;
    }

    public override void OnFinish()
    {
        targetParent.gameObject.SetActive(false);

        foreach (Transform t in objParent)
        {
            t.GetComponent<GateSide>().Interactable = false;
        }
    }
}
