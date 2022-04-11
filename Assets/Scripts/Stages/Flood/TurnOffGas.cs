using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnOffGas : Stage
{
    public GameObject interactArea;
    public GameObject fire;

    public override void OnBegin()
    {
        base.OnBegin();

        interactArea.gameObject.SetActive(true);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate
        {
            GameHandler.Singleton.StageFinish();
            interactArea.gameObject.SetActive(false);
        });
        interactArea.GetComponent<EventTrigger>().triggers.Add(entry);

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        AudioSource boil = a.PlaySound(a.boilWater);
        StartCoroutine(GameHandler.Singleton.Counter(5, delegate
       {
           boil.volume = .4f;
           a.PlaySound(a.turnOffGas);
       }));

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFinish()
    {
        fire.SetActive(false);
    }
}
