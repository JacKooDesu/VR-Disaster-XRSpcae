using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    public Glass[] glasses;

    int breakCount = 0;
    public int BreakCount
    {
        set
        {
            breakCount = value;
        }
        get
        {
            return breakCount;
        }
    }

    public LayerMask breakerLayer;

    private void OnEnable()
    {
        // breakerLayer = 1 << LayerMask.NameToLayer("Hover Check Layer");

        foreach (var g in glasses)
        {
            g.glassController = this;
            g.Setup();
        }
    }
}
