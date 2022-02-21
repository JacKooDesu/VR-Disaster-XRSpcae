using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GateMid : EventTrigger
{
    public List<Transform> targets = new List<Transform>();
    static int currentTarget = 0;
    public bool isSafe = false;

    private void OnEnable()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate { MoveToSafe(); });
        trigger.triggers.Add(entry);
    }

    public void BindTarget(List<Transform> t){
        targets = t;
    }

    public void MoveToSafe()
    {
        iTween.MoveTo(gameObject, targets[currentTarget].position, 1.2f);
        iTween.RotateTo(gameObject, targets[currentTarget].eulerAngles, 1.2f);

        isSafe = true;
        targets[currentTarget].gameObject.SetActive(false);

        GetComponent<Collider>().enabled = false;

        currentTarget++;
    }
}
