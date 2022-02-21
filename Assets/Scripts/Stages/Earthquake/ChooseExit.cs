using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChooseExit : Stage
{
    public UIQuickSetting failedUI;

    Vector3 originPlayerPosition;

    public Animator[] exits;

    public override void OnBegin()
    {
        base.OnBegin();
        originPlayerPosition = GameHandler.Singleton.player.transform.position;

        GameHandler.Singleton.player.SetCanMove(true);

        JacDev.Audio.Earthquake audio = (JacDev.Audio.Earthquake)GameHandler.Singleton.audioHandler;
        audio.PlaySound(audio.protectHead);

        foreach (StageObject s in stageObjects)
        {
            if (s.obj.GetComponentInChildren<EventTrigger>())
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                if (s.obj.name.Contains("Exit"))
                {
                    entry.callback.AddListener(delegate
                    {
                        GameHandler.Singleton.player.Teleport(s.obj.transform.position);
                        GameHandler.Singleton.StageFinish();
                    });
                }
                else
                {
                    entry.callback.AddListener(delegate
                    {
                        failedUI.TurnOn();
                        StartCoroutine(failedUI.WaitStatusChange(delegate
                        {
                            GameHandler.Singleton.BlurCamera(false);
                            GameHandler.Singleton.player.Teleport(originPlayerPosition);
                        }
                        ));
                    });
                }

                s.obj.GetComponentInChildren<EventTrigger>().triggers.Add(entry);
            }
        }
    }

    public override void OnUpdate()
    {
    }

    public override void OnFinish()
    {
        base.OnFinish();
        foreach (Animator a in exits)
        {
            a.enabled = true;
        }
    }
}
