using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kit : MonoBehaviour
{
    public InteracableObject[] items;
    public Transform itemParent;
    public Material hasTakenMaterial;

    private void Start()
    {
        for (int i = 0; i < items.Length; ++i)
        {
            var item = items[i];
        }
    }

    void SetMaterial(Transform t, Material mat, bool containChild = true)
    {
        if (t.GetComponent<Renderer>() != null)
        {
            for (int i = 0; i < t.GetComponent<Renderer>().sharedMaterials.Length; ++i)
                t.GetComponent<Renderer>().sharedMaterials[i] = mat;
        }

        if (!containChild)
            return;

        if (t.GetComponentsInChildren<Renderer>().Length == 0)
            return;

        foreach (var r in t.GetComponentsInChildren<Renderer>())
        {
            for (int i = 0; i < r.sharedMaterials.Length; ++i)
                r.sharedMaterials[i] = mat;

        }
    }
}
