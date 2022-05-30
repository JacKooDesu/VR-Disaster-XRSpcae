using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHandler : MonoBehaviour
{
    public Image hoverTimerUi;

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
}
