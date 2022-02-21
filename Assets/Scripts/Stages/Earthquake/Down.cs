using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class Down : Stage
{
    public UIQuickSetting UI;
    public float hintDisplayTime = 3f;
    public ObjectTweener tweener;

    public override void OnBegin()
    {
        base.OnBegin();

        UI.TurnOn();

        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.goUnderTable);
        audio.currentPlayingSound = null;

        GameHandler.Singleton.cam.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = true;

        GameHandler.Singleton.BlurCamera(true);

        StartCoroutine(GameHandler.Singleton.Counter(
            hintDisplayTime, hintDisplayTime,
            delegate
            {
                UI.TurnOff();
                GameHandler.Singleton.BlurCamera(false);
            }));

        if (spawnpoint != null)
            GameHandler.Singleton.SetLineGuider(true, spawnpoint.position);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        base.OnFinish();
        UI.TurnOff();
        tweener.MoveNextPoint();
    }
}
