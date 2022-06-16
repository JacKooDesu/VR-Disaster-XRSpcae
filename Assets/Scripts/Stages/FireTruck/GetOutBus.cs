using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutBus : Stage
{
    public InteracableObject door;
    public GameObject hintPoint;

    public override void OnBegin()
    {
        base.OnBegin();

        door.interactable = true;
        door.onHoverEvent.AddListener(() => isFinish = true);

        hintPoint.SetActive(true);
    }

    public override void OnFinish()
    {
        base.OnFinish();

        door.interactable = false;

        hintPoint.SetActive(false);
    }
}
