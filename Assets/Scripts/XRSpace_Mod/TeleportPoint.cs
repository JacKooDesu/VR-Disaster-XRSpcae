﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportPoint : MonoBehaviour
{
    public int materialColorIndex;
    public Color selectColor;
    public Color normalColor;
    [Range(0, 1)] public float colorLerpSpeed;

    bool isSelected = false;
    XRBaseRaycaster raycaster;

    // public System.Action onTeleportAction;
    public UnityEvent onTeleportAction;

    [Header("自訂屬性")]
    public string pointName;
    public bool showHint;

    public void BeingSelect(XRBaseRaycaster raycaster)
    {
        if (isSelected)
            return;

        isSelected = true;
        this.raycaster = raycaster;
        StartCoroutine(CheckSelecting());

        if (showHint)
        {
            FindObjectOfType<HintCanvas>().SetHintText($"傳送至{pointName}", true);
        }
    }

    IEnumerator CheckSelecting()
    {
        while (raycaster.HitResult.gameObject == gameObject)
        {
            yield return null;
        }
        isSelected = false;
        FindObjectOfType<HintCanvas>().ShowHintText(false);
    }

    public void BeingTeleported()
    {
        print($"{gameObject.name} being teleport!");

        onTeleportAction.Invoke();
    }

    private void Update()
    {
        LerpColor();
    }

    void LerpColor()
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        var m = mr.sharedMaterials[materialColorIndex];
        if (isSelected)
            m.color = Color.Lerp(m.color, selectColor, colorLerpSpeed);
        else
            m.color = Color.Lerp(m.color, normalColor, colorLerpSpeed);
    }
}
