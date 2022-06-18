using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SpecRainMeter : Stage
{
    public InteracableObject interactArea;
    public GameObject ui;

    public override void OnBegin()
    {
        interactArea.interactable = true;
        interactArea.interactableOutline = false;
        interactArea.GetComponent<Outline>().enabled = true;
        interactArea.onHoverEvent.AddListener(() => StartCoroutine(ShowUI()));
    }

    IEnumerator ShowUI()
    {
        interactArea.interactable = false;
        ui.SetActive(true);
        ui.GetComponentInChildren<UIQuickSetting>().TurnOn();

        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.specWaterMeter);

        yield return new WaitForSeconds(a.specWaterMeter.length);

        ui.GetComponentInChildren<UIQuickSetting>().TurnOff();
        GameHandler.Singleton.StageFinish();
    }
}
