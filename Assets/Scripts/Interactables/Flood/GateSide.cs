using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GateSide : MonoBehaviour
{
    public Transform target;
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
        iTween.MoveTo(gameObject, target.position, 1.2f);
        iTween.RotateTo(gameObject, target.eulerAngles, 1.2f);
        target.gameObject.SetActive(false);

        isSafe = true;

        GetComponent<Collider>().enabled = false;
    }
}
