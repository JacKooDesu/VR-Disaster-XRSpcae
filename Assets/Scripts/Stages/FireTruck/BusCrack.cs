using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusCrack : Stage
{
    public float minTime;
    public float maxTime;

    public float crashTime = 5f;

    public Transform jointer;
    public Transform headTransform;

    public override void OnBegin()
    {
        JacDev.Audio.FireTruck audio = (JacDev.Audio.FireTruck)GameHandler.Singleton.audioHandler;
        AudioSource a = audio.PlayAudio(audio.bgm1, true, GameHandler.Singleton.player.transform);
        a.volume = .2f;

        GameHandler.Singleton.player.SetCanMove(false);
        headTransform = GameHandler.Singleton.player.head;

        StartCoroutine(GameHandler.Singleton.Counter(minTime, maxTime, delegate
        {
            Crash();
            Destroy(a.gameObject);
        }));
    }

    public override void OnFinish()
    {
    }

    public void Crash()
    {
        // print("crash");

        Player p = GameHandler.Singleton.player;
        p.canRotate = false;
        jointer.parent.position = GameHandler.Singleton.player.head.position;
        Transform originParent = GameHandler.Singleton.player.transform;
        headTransform.SetParent(jointer);

        Rigidbody rb = jointer.GetComponent<Rigidbody>();
        rb.AddForce(jointer.forward * 5, ForceMode.Impulse);

        StartCoroutine(
            GameHandler.Singleton.Counter(
                crashTime,
                crashTime,
                delegate
                {
                    headTransform.SetParent(originParent);
                    jointer.gameObject.SetActive(false);
                    //p.SetCanMove(true);

                    GameHandler.Singleton.StageFinish();
                    GameHandler.Singleton.player.canRotate = true;
                }));
    }
}
