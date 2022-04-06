using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteracableObject : MonoBehaviour
{
    [Header("位置重置")]
    public bool positionReset = true;
    public float resetTime;
    protected float timer;
    protected Vector3 originPos;
    protected Quaternion originRotation;
    public UnityEvent onGrabEvent;
    public UnityEvent onReleaseEvent;
    protected bool isGrabbing;

    public bool interactable = true;


    protected virtual void OnEnable()
    {
        if (positionReset)
            originPos = transform.position;
    }

    protected virtual void Update()
    {
        if (!interactable && isGrabbing)
            Released();
        CheckPosReset();
    }

    protected void CheckPosReset()
    {
        if (!positionReset)
            return;

        if (isGrabbing)
            return;

        timer += Time.deltaTime;
        if (timer >= resetTime)
            ResetPosition();
    }

    protected void ResetPosition()
    {
        timer = 0f;

        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        transform.SetPositionAndRotation(originPos, originRotation);
    }

    public void Grabbed()
    {
        if (isGrabbing)
            return;

        onGrabEvent.Invoke();
        isGrabbing = true;
    }

    public void Released()
    {
        print($"Has Release {transform.GetInstanceID()}");
        if (!isGrabbing)
            return;

        onReleaseEvent.Invoke();
        isGrabbing = false;
    }
}
