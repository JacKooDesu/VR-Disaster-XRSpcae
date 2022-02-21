using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlowerpot : Stage
{
    public Plant[] plants;


    public override void OnBegin()
    {
        base.OnBegin();
        foreach (Plant p in plants)
        {
            p.GetComponent<Collider>().enabled = true;
        }

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        AudioSource broadcast = a.PlaySound(a.broadcast1);
        StartCoroutine(
            GameHandler.Singleton.Counter(
                3f,
                delegate
                {
                    broadcast.volume = .4f;
                    a.PlaySound(a.plantDown);
                }
            ));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (Plant p in plants)
        {
            if (!p.isSafe)
                return;
        }

        GameHandler.Singleton.StageFinish();
    }

    public override void OnFinish()
    {
        foreach (Plant p in plants)
        {
            p.GetComponent<Collider>().enabled = false;
        }
    }
}
