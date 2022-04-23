﻿using System.Collections;
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
    public CameraFadeUtil fadeUtil;

    GameObject target;
    public bool hasTarget;  //是否有目標物

    public bool canMove = true;
    public bool canRotate = true;
    public float moveDistance = 100f;
    public bool isTeleport = false;
    public Vector3 teleportTarget;

    float originHeight;

    public CurveLineRenderer curveLine;

    public Kit kit;

    [Header("控制器")]
    public Transform leftHandler, rightHandler;

    public Transform foot;

    // Overlay Effect 設定
    UnityStandardAssets.ImageEffects.ScreenOverlay[] overlays;
    static float overlayOriginValue;

    [Header("Navigator")]
    public NavMeshAgent agent;
    public LineRendererUtil line;

    public UnityEngine.Events.UnityEvent onTeleportEvent;

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
        fadeUtil.FadeIn(.5f);

        onTeleportEvent.AddListener(() => agent.Warp(transform.position));
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
        fadeUtil.FadeOutIn(
            .5f,
            () => SetCanMove(false),
            () => { transform.position = point; },
            () => SetCanMove(true));
    }

    public void PathFinding(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPos, path);

        line.SetCorners(path.corners);
    }

    public void ShowKit(bool b = true)
    {
        kit.gameObject.SetActive(b);
    }
}
