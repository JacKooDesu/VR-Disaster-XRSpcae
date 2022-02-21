using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Plant : MonoBehaviour
{
    public Transform safePlace;
    public bool isSafe = false;

    private void OnEnable()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate { MoveToSafe(); });
        trigger.triggers.Add(entry);
    }

    public void MoveToSafe()
    {
        iTween.MoveTo(gameObject, safePlace.position, 1.2f);
        isSafe = true;
    }
}
