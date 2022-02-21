using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BrokenGlass : MonoBehaviour
{
    public Material originMaterial;
    public Material hintMaterial;

    public GlassController glassController;

    private void OnEnable()
    {
        BindGlassController();
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(delegate { Hit(); });
        trigger.triggers.Add(entry);
    }

    private void Hit()
    {
        JacDev.Audio.AudioHandler audio = GameHandler.Singleton.audioHandler;
        audio.PlayAudio(audio.soundList.glassBreak, false, transform);

        GetComponent<Rigidbody>().isKinematic = false;

        glassController.AddBreakGlass();
        StartCoroutine(GameHandler.Singleton.Counter(3f, 3f, delegate { Destroy(gameObject); }));
    }

    public void BindGlassController()
    {
        glassController = FindObjectOfType<GlassController>() as GlassController;
    }
}
