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
    public GameObject currentActiveCanvas;
    List<GameObject> handlingCanvas = new List<GameObject>();

    private void Start()
    {
        currentActiveCanvas = transform.GetChild(0).gameObject;
    }

    public void SetHintText(string str, bool show, bool forceToForward = true)
    {
        SetHintText(str);
        ShowHintText(show);
    }

    public void ShowHintText(bool show, bool forceToForward = true)
    {
        hintText.gameObject.SetActive(show);
        if (forceToForward)
            StartCoroutine(LerpToHeadAngle());
    }

    public void SetHintText(string str)
    {
        hintText.text = str;
    }

    public GameObject AddCanvs(GameObject prefab)
    {
        var g = Instantiate(prefab, transform);
        handlingCanvas.Add(g);
        currentActiveCanvas = g;

        return g;
    }

    private void Update()
    {
        if (currentActiveCanvas == null)
        {
            var index = handlingCanvas.Count;
            handlingCanvas.RemoveAt(index - 1);
            currentActiveCanvas = handlingCanvas[index - 2];
        }

        CheckTracking();
    }

    #region Tracking
    void CheckTracking()
    {
        if (!headTracking)
            return;

        if (Mathf.Abs(Mathf.DeltaAngle(head.eulerAngles.y, transform.eulerAngles.y)) > trackingAngle)
            trackingDelayCounter += Time.timeScale != 0 ? Time.deltaTime : Time.unscaledDeltaTime;
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
    #endregion
}
