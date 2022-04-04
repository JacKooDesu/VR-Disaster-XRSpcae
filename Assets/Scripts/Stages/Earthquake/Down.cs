using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class Down : Stage
{
    [SerializeField]
    float height = 1.3f;
    public UIQuickSetting UI;
    public float hintDisplayTime = 3f;

    public MaterialChanger changer;

    public override void OnBegin()
    {
        base.OnBegin();
        GameHandler.Singleton.player.hintCanvas.SetHintText("請趴低找掩護", true, true);
        changer.ChangeColor();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (GameHandler.Singleton.player.head.position.y < height)
        {
            isFinish = true;
        }
    }

    public override void OnFinish()
    {
        base.OnFinish();
        // UI.TurnOff();
        // tweener.MoveNextPoint();
        changer.BackOriginColor();
    }
}
