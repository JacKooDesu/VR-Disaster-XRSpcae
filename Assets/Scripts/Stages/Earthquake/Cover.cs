using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : Stage
{
    public UIQuickSetting UI;
    public float hintDisplayTime = 3f;
    public ObjectTweener tweener;
    public override void OnBegin()
    {
        base.OnBegin();
        GameHandler.Singleton.player.SetCanMove(false);

        UI.TurnOn();
        GameHandler.Singleton.BlurCamera(true);

        StartCoroutine(GameHandler.Singleton.Counter(
            hintDisplayTime, hintDisplayTime,
            delegate
            {
                UI.TurnOff();
                GameHandler.Singleton.BlurCamera(false); ;
            }));
    }

    public override void OnFinish()
    {
        base.OnFinish();
        UI.TurnOff();
        tweener.MoveNextPoint();
    }
}
