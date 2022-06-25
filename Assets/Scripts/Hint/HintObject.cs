﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HintObject : MonoBehaviour
{
    public float limitTime = 5f;
    public float hasRenderTime;    // 已經被偵測到的秒數
    public float minDst = 0;    // 少於這個距離就不再顯示
    public bool isRendering = false;
    HintUI ui;

    [Header("UI設定")]
    public string objectName;

    protected HintObejctCamera cam;
    // public Sprite image;

    private void Start()
    {
        ui = FindObjectOfType<HintUI>();
        cam = FindObjectOfType<HintObejctCamera>();

#if UNITY_EDITOR
        if (Application.isEditor)
            SceneVisibilityManager.instance.Hide(gameObject, true);
#endif
    }

    private void OnWillRenderObject()
    {
        if (!isRendering)
            isRendering = true;
    }

    private void OnBecameInvisible()
    {
        isRendering = false;
        hasRenderTime = 0;
        ui.Hide();
    }

    private void OnDisable()
    {
        ui?.Hide();
    }

    private void Update()
    {
        if (Vector3.Distance(cam.transform.position, transform.position) < minDst)
        {
            ui.Hide();
            return;
        }

        if (isRendering)
        {
            if (hasRenderTime >= limitTime)
                ui.Show(this);
            hasRenderTime += Time.deltaTime;
        }
    }
}
