using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitItem : InteracableObject
{
    public bool inPack = false;

    protected override void Start()
    {
        base.Start();
        onReleaseEvent.AddListener(() => { new CoroutineUtility.Timer(3f, () => ResetPosition()); });
    }

    protected override void Update()
    {
        base.Update();
        if (!isGrabbing)
            transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
