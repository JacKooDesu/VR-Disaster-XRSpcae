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

    bool hasHit;

    private void Hit()
    {
        JacDev.Audio.AudioHandler audio = GameHandler.Singleton.audioHandler;
        audio.PlayAudio(audio.soundList.glassBreak, false, transform);

        GetComponent<Rigidbody>().isKinematic = false;

        glassController.BreakCount++;

        var timer = new CoroutineUtility.Timer(3f, () => Destroy(gameObject));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;
        if (other.gameObject.layer != glassController.breakerLayer)
            return;

        Hit();
    }
}
