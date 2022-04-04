using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Transform head;

    public bool isStop = true;

    float counter = 0;
    float stopTime = 3f;

    Rigidbody rb;

    public HintCanvas hintCanvas;

    GameObject target;
    public bool hasTarget;  //是否有目標物

    public bool canMove = true;
    public bool canRotate = true;
    public float moveDistance = 100f;
    public bool isTeleport = false;
    public Vector3 teleportTarget;

    float originHeight;

    public CurveLineRenderer curveLine;

    [Header("控制器")]
    public Transform leftHandler, rightHandler;

    public Transform foot;

    // Overlay Effect 設定
    UnityStandardAssets.ImageEffects.ScreenOverlay[] overlays;
    static float overlayOriginValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        hintCanvas.head = head;

        // curveLine.gameObject.SetActive(false);
        originHeight = transform.position.y;

        if (GetComponentInChildren<NavMeshAgent>() != null)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -transform.up, out hit);
            NavMeshAgent agent = GetComponentInChildren<NavMeshAgent>();
            agent.baseOffset = Vector3.Distance(transform.position, hit.point);
        }

        SetupOverlayEffect();
        CameraFadeIn();
    }

    void SetupOverlayEffect()
    {
        overlays = head.GetComponentsInChildren<UnityStandardAssets.ImageEffects.ScreenOverlay>();
        overlayOriginValue = overlays[0].intensity;
    }

    public void SetTarget(GameObject target)
    {
        hasTarget = true;
        this.target = target;
    }

    public void SetCanMove(bool b)
    {
        // if (rb == null)
        //     rb = GetComponent<Rigidbody>();

        canMove = b;
        //rb.isKinematic = !b;
    }

    public void Teleport(Vector3 point)
    {
        CameraFadeOutIn(
            () => SetCanMove(false),
            () => { transform.position = point; },
            () => SetCanMove(true));
    }

    public void CameraFadeOutIn(System.Action beginAction = null, System.Action blackAction = null, System.Action finishedAction = null)
    {
        if (beginAction != null)
            beginAction.Invoke();

        foreach (var overlay in overlays)
        {
            overlay.enabled = true;
            DOTween.To(() => overlay.intensity, f => overlay.intensity = f, 0f, .8f).OnComplete(
                () =>
                {
                    DOTween.To(() => overlay.intensity, f => overlay.intensity = f, overlayOriginValue, .8f).OnComplete(
                    () =>
                    {
                        overlay.enabled = false;
                        if (finishedAction != null)
                            finishedAction.Invoke();
                    });
                    if (blackAction != null)
                        blackAction.Invoke();
                }
            );
        }
    }

    public void CameraFadeOut(System.Action beginAction = null, System.Action finishedAction = null)
    {
        if (beginAction != null)
            beginAction.Invoke();

        foreach (var overlay in overlays)
        {
            overlay.enabled = true;
            DOTween.To(() => overlay.intensity, f => overlay.intensity = f, 0f, .8f).OnComplete(
                () =>
                {
                    if (finishedAction != null)
                        finishedAction.Invoke();
                }
            );
        }
    }

    public void CameraFadeIn(System.Action beginAction = null, System.Action finishedAction = null)
    {
        if (beginAction != null)
            beginAction.Invoke();

        foreach (var overlay in overlays)
        {
            overlay.intensity = 0;
            overlay.enabled = true;
            DOTween.To(() => overlay.intensity, f => overlay.intensity = f, overlayOriginValue, .8f).OnComplete(
                () =>
                {
                    if (finishedAction != null)
                        finishedAction.Invoke();
                }
            );
        }
    }
}
