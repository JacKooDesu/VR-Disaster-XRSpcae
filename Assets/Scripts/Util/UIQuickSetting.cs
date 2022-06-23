﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIQuickSetting : MonoBehaviour
{
    public bool fadeIn;
    public bool fadeOut;

    CanvasGroup canvasGroup;

    public bool bindSound = true;

    public bool hideAtStart = true;

    bool status;
    public bool Status
    {
        private set
        {
            canvasGroup.blocksRaycasts = value;
            status = value;
        }
        get => status;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (bindSound)
            BindButton();

        if (hideAtStart)
        {
            canvasGroup.alpha = 0;
            Status = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void TurnOn()
    {
        if (fadeIn)
        {
            StartCoroutine(FadingIn());
        }
    }

    public void TurnOff()
    {
        if (fadeOut)
        {
            StartCoroutine(FadingOut());
        }
    }

    void BindButton()
    {
        Button[] tempButtons = GetComponentsInChildren<Button>();

        foreach (Button b in tempButtons)
        {
            // print(b.gameObject);
            GameHandler.Singleton.BindEvent(
                b.gameObject,
                EventTriggerType.PointerEnter,
                delegate
                {
                    JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.select);
                    //GameHandler.Singleton.BlurCamera(true);
                });

            GameHandler.Singleton.BindEvent(
                b.gameObject,
                EventTriggerType.PointerDown,
                delegate
                {
                    JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.hover);
                    //GameHandler.Singleton.BlurCamera(false);
                });

            // GameHandler.Singleton.BindEvent(
            //     b.gameObject,
            //     EventTriggerType.PointerExit,
            //     delegate { GameHandler.Singleton.BlurCamera(false); }
            // );
        }

        // if (GetComponent<Button>())
        // {
        //     GameHandler.Singleton.BindEvent(
        //         gameObject,
        //         EventTriggerType.PointerEnter,
        //         delegate
        //         {
        //             JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.select);
        //         });

        //     GameHandler.Singleton.BindEvent(
        //         gameObject,
        //         EventTriggerType.PointerDown,
        //         delegate
        //         {
        //             JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.hover);
        //         });
        // }

    }

    IEnumerator FadingIn()
    {
        Status = true;
        while (Mathf.Abs(canvasGroup.alpha - 1) > 0.01f && gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, .1f);
            yield return null;
        }
    }

    IEnumerator FadingOut()
    {
        Status = false;
        while (Mathf.Abs(canvasGroup.alpha - 0) > 0.01f && gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, .1f);
            yield return null;
        }
    }

    public IEnumerator WaitStatusChange(UnityAction action, bool status)
    {
        while (Status != status)
        {
            yield return null;
        }
        // Debug.Log("bool change");
        action.Invoke();
    }
}
