using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintCanvas : MonoBehaviour
{
    [HideInInspector] public Transform head;
    bool headTracking = true;
    public bool HeadTracking
    {
        get => headTracking;
        set { headTracking = value; }
    }

    public float trackingDelay = 3f;
    float trackingDelayCounter = 0f;

    public float trackingAngle;

    public Text hintText;

    public void SetHintText(string str, bool show)
    {
        SetHintText(str);
        ShowHintText(show);
    }

    public void ShowHintText(bool show)
    {
        hintText.gameObject.SetActive(show);
    }

    public void SetHintText(string str)
    {
        hintText.text = str;
    }

    private void Update()
    {
        if (!headTracking)
            return;

        if (Mathf.Abs(Mathf.DeltaAngle(head.eulerAngles.y, transform.eulerAngles.y)) > trackingAngle)
            trackingDelayCounter += Time.deltaTime;
        else
            trackingDelayCounter = 0f;


        if (trackingDelayCounter >= trackingDelay)
        {
            StartCoroutine(LerpToHeadAngle());
        }
    }

    IEnumerator LerpToHeadAngle()
    {
        Vector3 origin = transform.eulerAngles;
        float targetAngleAmount = Mathf.DeltaAngle(transform.eulerAngles.y, head.eulerAngles.y);
        float yAngle = 0;
        while (Mathf.Abs(yAngle - targetAngleAmount) >= .001f)
        {
            yAngle = Mathf.Lerp(yAngle, targetAngleAmount, .1f);
            transform.eulerAngles = Vector3.up * yAngle + origin;

            trackingDelayCounter = 0f;
            
            yield return null;
        }
    }
}
