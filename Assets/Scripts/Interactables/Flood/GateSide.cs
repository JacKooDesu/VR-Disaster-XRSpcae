﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSide : InteracableObject
{
    public bool hasInstalled;
    public Transform targetParent;
    List<Transform> targets = new List<Transform>();

    protected override void Start()
    {
        base.Start();
        foreach (Transform t in targetParent)
            targets.Add(t);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject == null) return;

        Transform t;
        if (targets.Contains(t = other.transform))
        {
            transform.SetPositionAndRotation(t.position, t.rotation);
            positionReset = false;
            interactable = false;
            hasInstalled = true;

            t.gameObject.SetActive(false);
        }
    }
}
