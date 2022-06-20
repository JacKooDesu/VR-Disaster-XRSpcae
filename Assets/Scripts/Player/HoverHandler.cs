﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHandler : MonoBehaviour
{
    public Image hoverTimerUi;
    public bool mainMenuEnable;
    public float mainMenuTime = 5f;
    public bool debugMode = false;
    static bool isCallingMenu;  // 雙手合十，回主選單
    static float timer;     // 秒數回主選單

    private void Start()
    {
        ResetImage();
    }

    public void UpdateImage(float t)
    {
        hoverTimerUi.fillAmount = t;
    }

    public void ResetImage()
    {
        hoverTimerUi.fillAmount = 0;
    }

    private void Update()
    {
        if (isCallingMenu)
        {
            timer += Time.deltaTime * .5f;
            UpdateImage(timer / mainMenuTime);

            if (timer >= mainMenuTime)
            {
                FindObjectOfType<AsyncLoadingScript>().LoadScene("MissionSelect");
                isCallingMenu = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mainMenuEnable)
            return;

        if (isCallingMenu)
            return;

        if (Application.isEditor && !debugMode)
            return;

        if (other.gameObject.layer == gameObject.layer)
        {
            isCallingMenu = true;
            timer = 0f;

            foreach (var hh in FindObjectsOfType<HoverHandler>())
                hh.ResetImage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!mainMenuEnable)
            return;

        if (!isCallingMenu)
            return;

        if (Application.isEditor && !debugMode)
            return;

        if (other.gameObject.layer == gameObject.layer)
        {
            isCallingMenu = false;
            timer = 0f;
            
            foreach (var hh in FindObjectsOfType<HoverHandler>())
                hh.ResetImage();
        }
    }
}
