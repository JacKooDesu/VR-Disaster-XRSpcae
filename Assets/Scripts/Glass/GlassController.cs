using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    public GameObject originParent;
    public GameObject brokenParent;

    Dictionary<string, GameObject> originGlassList = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> brokenGlassList = new Dictionary<string, GameObject>();

    public UnityEngine.Events.UnityEvent brokenAction;

    int breakCount = 0;
    public bool hasBreaker = false;
    public bool HasBreaker
    {
        set
        {
            hasBreaker = value;
        }
        get
        {
            return hasBreaker;
        }
    }

    public void BindGlass()
    {
        for (int i = 0; i < originParent.transform.childCount; ++i)
        {
            Transform originTemp = originParent.transform.GetChild(i);
            Transform brokenTemp = brokenParent.transform.GetChild(i);

            originGlassList.Add(originTemp.name, originTemp.gameObject);
            brokenGlassList.Add(brokenTemp.name, brokenTemp.gameObject);
        }

        foreach (GameObject g in originGlassList.Values)
        {
            foreach (Transform t in g.transform)
            {
                t.gameObject.SetActive(true);
                Glass tempGlass = t.gameObject.AddComponent<Glass>();
                print(g.name);
                tempGlass.SetGlass(g.transform, brokenGlassList[g.name].transform, this);
            }

        }
    }

    public void AddBreakGlass()
    {
        breakCount++;
    }

    public int GetBreakCount()
    {
        return breakCount;
    }
}
