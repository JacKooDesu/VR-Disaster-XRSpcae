using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalGateMid : Stage
{
    public GameObject spotlight;
    public Transform objParent;
    [SerializeField] List<Transform> targets = new List<Transform>();

    public override void OnBegin()
    {
        foreach (Transform t in objParent)
        {
            var interact = t.GetComponent<GateMid>();
            interact.interactable = true;
            interact.targets = targets;
        }

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.instalGateMid);

        targets[0].gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (Transform t in objParent)
        {
            if (!t.GetComponent<GateMid>().hasInstalled)
                return;
        }
        isFinish = true;
    }

    public override void OnFinish()
    {
        foreach (Transform t in objParent)
        {
            t.GetComponent<GateMid>().interactable = false;
        }

        spotlight.SetActive(false);
    }
}
