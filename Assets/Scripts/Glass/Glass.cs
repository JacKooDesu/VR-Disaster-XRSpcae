using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Glass : MonoBehaviour
{
    [HideInInspector] public GlassController glassController;

    public Transform origin;
    public Transform brokenParent;

    // public List<BrokenGlass> brokenGlasses = new List<BrokenGlass>();
    public InteracableObject[] breakPoints;

    public void Setup()
    {
        EnableBreaker(false);
        BindBreakPoints();
        BindBrokenGlass();
    }

    public void EnableBreaker(bool enable)
    {
        foreach (var bPoint in breakPoints)
            bPoint.interactable = enable;
    }

    void BindBreakPoints()
    {
        foreach (var bPoint in breakPoints)
        {
            bPoint.onHoverEvent.AddListener(GlassBroken);
        }
    }

    void BindBrokenGlass()
    {
        foreach (Transform brokenG in brokenParent)
        {
            var b = brokenG.gameObject.AddComponent<BrokenGlass>();
            b.glassController = glassController;
        }
    }

    void GlassBroken()
    {
        JacDev.Audio.AudioHandler audio = GameHandler.Singleton.audioHandler;
        audio.PlayAudio(audio.soundList.glassBreak, false, transform);

        origin.gameObject.SetActive(false);

        brokenParent.gameObject.SetActive(true);
    }
}
