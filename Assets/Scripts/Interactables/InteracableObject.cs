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
    protected Transform originParent;
    public UnityEvent onGrabEvent;
    public UnityEvent onReleaseEvent;
    protected bool isGrabbing;
    public bool IsGrabbing
    {
        get => isGrabbing;
    }

    public bool interactable = true;

    // rigidbody 設定
    protected bool originIsKinematic;
    protected bool originUseGravity;

    protected virtual void Start()
    {
        SetupOrigin();
    }

    // 定義原位置訊息
    protected void SetupOrigin()
    {
        originPos = transform.position;
        originRotation = transform.rotation;
        originParent = transform.parent;

        if (!GetComponent<Rigidbody>())
            return;

        var rig = GetComponent<Rigidbody>();
        originIsKinematic = rig.isKinematic;
        originUseGravity = rig.useGravity;
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

    protected virtual void ResetPosition()
    {
        timer = 0f;

        transform.parent = originParent;

        if (GetComponent<Rigidbody>() != null)
        {
            var rig = GetComponent<Rigidbody>();
            rig.velocity = Vector3.zero;
            rig.isKinematic = originIsKinematic;
            rig.useGravity = originUseGravity;
        }

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

    private void OnMouseEnter()
    {

    }
}
