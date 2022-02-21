using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Glass : MonoBehaviour
{
    Transform origin;
    Transform broken;
    GlassController glassController;

    public void SetGlass(Transform origin, Transform broken, GlassController g)
    {
        this.origin = origin;
        this.broken = broken;
        this.glassController = g;
    }

    private void OnEnable()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate { GlassBroken(); });
        trigger.triggers.Add(entry);
    }

    void GlassBroken()
    {
        if (glassController.HasBreaker)
        {
            JacDev.Audio.AudioHandler audio = GameHandler.Singleton.audioHandler;
            audio.PlayAudio(audio.soundList.glassBreak, false, transform);
            broken.gameObject.SetActive(true);

            origin.gameObject.SetActive(false);

            glassController.brokenAction.Invoke();
        }

    }
}
