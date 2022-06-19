using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpecRainMeter : Stage
{
    public InteracableObject interactArea;
    public GameObject ui;

    [Header("UI設定")]
    public Button[] selections;
    public string[] selectionHints;
    public Text hintText;
    public Color mainColor;
    public Button ConfirmBtn;
    int currentSelectIndex;

    public override void OnBegin()
    {
        interactArea.interactable = true;
        interactArea.interactableOutline = false;
        interactArea.GetComponent<Outline>().enabled = true;
        interactArea.onHoverEvent.AddListener(() => StartCoroutine(ShowUI()));

        GameHandler.Singleton.player.PathFinding(interactArea.transform.position);

        BindButtons();
    }

    void BindButtons()
    {
        for (int i = 0; i < selections.Length; ++i)
        {
            int x = i;
            var btn = selections[i];
            var s = selectionHints[i];

            btn.onClick.AddListener(
                () =>
                {
                    foreach (var b in selections)
                        b.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                    btn.GetComponent<Image>().color = mainColor;

                    hintText.text = s;

                    currentSelectIndex = x;

                    ConfirmBtn.interactable = true;
                }
            );
        }

        ConfirmBtn.onClick.AddListener(() => isFinish = true);
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

    public override void OnFinish()
    {
        base.OnFinish();
        GameHandler.Singleton.player.line.gameObject.SetActive(false);
    }
}
