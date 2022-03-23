using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintObject : MonoBehaviour
{
    public float hasRenderTime;    // 已經被偵測到的秒數
    public bool isRendering = false;
    public HintUI ui;

    [Header("UI設定")]
    public string objectName;
    public Sprite image;
    
    private void OnWillRenderObject()
    {
        if (!isRendering)
            isRendering = true;
    }

    private void OnBecameInvisible()
    {
        isRendering = false;
        hasRenderTime = 0;
    }

    private void Update()
    {
        if (isRendering)
            hasRenderTime += Time.deltaTime;
    }
}
