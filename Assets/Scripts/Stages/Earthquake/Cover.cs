using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : Stage
{
    [SerializeField] Transform table;
    [SerializeField] float radius;
    public MaterialChanger changer;

    public override void OnBegin()
    {
        base.OnBegin();
        changer.ChangeColor();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var headPos = GameHandler.Singleton.player.head.position;
        var targetPos = table.position;
        if ((headPos - targetPos).magnitude < radius)
            isFinish = true;

    }

    public override void OnFinish()
    {
        base.OnFinish();
        changer.BackOriginColor();
    }
}
