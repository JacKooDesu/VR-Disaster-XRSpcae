using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalGateMid : Stage
{
    public Transform midTarget;
    public Transform midObject;

    public override void OnBegin()
    {
        midTarget.gameObject.SetActive(true);

        midTarget.GetChild(0).gameObject.SetActive(true);

        List<Transform> temps = new List<Transform>();
        for (int i = 0; i < midTarget.childCount; ++i)
            temps.Add(midTarget.GetChild(i));

        foreach (Transform t in midObject)
        {
            t.GetComponent<Collider>().enabled = true;
            t.GetComponent<GateMid>().BindTarget(temps);
        }

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.instalGateMid);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (Transform t in midObject)
        {
            if (!t.GetComponent<GateMid>().isSafe)
                return;
        }

        GameHandler.Singleton.StageFinish();
    }

    public override void OnFinish()
    {
        midTarget.gameObject.SetActive(false);

        foreach (Transform t in midObject)
        {
            t.GetComponent<Collider>().enabled = false;
        }
    }
}
