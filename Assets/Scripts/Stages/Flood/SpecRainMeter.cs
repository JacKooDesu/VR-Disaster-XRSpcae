using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SpecRainMeter : Stage
{
    public GameObject interactArea;
    public GameObject ui;

    public override void OnBegin()
    {
        interactArea.SetActive(true);
        EventTrigger trigger = interactArea.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate { StartCoroutine(ShowUI()); });
        trigger.triggers.Add(entry);
    }

    IEnumerator ShowUI()
    {
        interactArea.SetActive(false);
        ui.SetActive(true);
        ui.GetComponentInChildren<UIQuickSetting>().TurnOn();

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.specWaterMeter);

        yield return new WaitForSeconds(a.specWaterMeter.length);

        ui.GetComponentInChildren<UIQuickSetting>().TurnOff();
        GameHandler.Singleton.StageFinish();
    }
}
