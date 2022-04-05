using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteracableObject : MonoBehaviour
{
    [Header("位置重置")]
    public bool positionReset = true;
    public float resetTime;
    float timer;
    Vector3 originPos;
    Quaternion originRotation;
    public UnityEvent onGrabEvent;
    public UnityEvent onReleaseEvent;
    bool isGrabbing;


    private void OnEnable()
    {
        if (positionReset)
            originPos = transform.position;
    }

    void Update()
    {
        if (!positionReset)
            return;

        if (isGrabbing)
            return;

        timer += Time.deltaTime;
        if (timer >= resetTime)
            ResetPosition();
    }

    void ResetPosition()
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
        if (!isGrabbing)
            return;

        onReleaseEvent.Invoke();
        isGrabbing = false;
    }
}
