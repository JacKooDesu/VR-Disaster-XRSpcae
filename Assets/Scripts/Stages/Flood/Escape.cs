using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : Stage
{
    public GameObject water;
    public GameObject waterfall;
    public GameObject brokenCell;

    public override void OnBegin()
    {
        base.OnBegin();

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.waterIn);

        StartCoroutine(
            GameHandler.Singleton.Counter(
                a.waterIn.length,
                delegate
                {
                    a.PlaySound(a.broadcast2);
                }
            ));

        StartCoroutine(
            GameHandler.Singleton.Counter(
                a.waterIn.length + 5f,
                delegate
                {
                    a.GetSoundAudioSource(a.broadcast2).volume = .4f;
                    a.PlaySound(a.escape);
                    if (spawnpoint != null)
                        GameHandler.Singleton.SetLineGuider(true, spawnpoint.position);
                }
            )
        );

        water.SetActive(true);
        iTween.MoveTo(water, Vector3.one * -1.46f, 12f);

        waterfall.SetActive(true);

        brokenCell.SetActive(false);
    }

    public override void OnUpdate()
    {
        if (spawnpoint != null)
            if ((GameHandler.Singleton.player.transform.position - spawnpoint.position).magnitude < 2.5f)
            {
                GameHandler.Singleton.SetLineGuider(false);
                GameHandler.Singleton.StageFinish();
            }

    }
    public override void OnFinish()
    {
        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.stageClear);

        GameHandler.Singleton.SetLineGuider(false);
    }
}
