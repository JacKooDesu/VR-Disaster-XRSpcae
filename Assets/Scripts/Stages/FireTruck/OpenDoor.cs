using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineUtility;
public class OpenDoor : Stage
{
    public InteracableObject door;
    public GameObject hintPoint;
    public GameObject informUi;

    Timer uiTimer;

    public override void OnBegin()
    {
        base.OnBegin();

        door.interactable = true;
        door.onHoverEvent.AddListener(() => isFinish = true);

        hintPoint.SetActive(true);

        uiTimer = new Timer(
            5f,
            () => informUi.SetActive(true),
            (f) => { },
            () => informUi.SetActive(false));

        GameHandler.Singleton.player.PathFinding(hintPoint.transform.position);
    }

    public override void OnFinish()
    {
        base.OnFinish();

        door.interactable = false;

        hintPoint.SetActive(false);

        uiTimer.Stop(true);
        GameHandler.Singleton.player.line.gameObject.SetActive(false);
    }
}
