using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldOn : Stage
{
    public UIQuickSetting UI;
    public UIQuickSetting waringHUD;
    public Transform tableTop;
    public float hintDisplayTime = 3f;

    public ObjectTweener tweener;
    public override void OnBegin()
    {
        base.OnBegin();

        UI.TurnOn();
        GameHandler.Singleton.BlurCamera(true);
        StartCoroutine(GameHandler.Singleton.Counter(
            hintDisplayTime,
            delegate
            {
                UI.TurnOff();
                GameHandler.Singleton.BlurCamera(false);
            }));
    }

    public override void OnUpdate()
    {
        if (GameHandler.Singleton.cam.transform.position.y > tableTop.position.y)
        {
            if (!waringHUD.gameObject.activeInHierarchy)
            {
                waringHUD.gameObject.SetActive(true);
            }
            waringHUD.TurnOn();
            GameHandler.Singleton.BlurCamera(true);
        }
        else
        {

            waringHUD.TurnOff();
            GameHandler.Singleton.BlurCamera(false);
        }
    }



    public override void OnFinish()
    {
        base.OnFinish();

        UI.TurnOff();

        // 2021.03.11 
        UI.transform.parent.gameObject.SetActive(false);
        GameHandler.Singleton.cam.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = false;

        tweener.MoveNextPoint();
    }
}
