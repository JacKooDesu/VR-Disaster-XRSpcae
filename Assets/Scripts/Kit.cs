﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.UI;

public class Kit : MonoBehaviour
{
    public List<KitItem> items = new List<KitItem>();
    List<KitItem> correctItems = new List<KitItem>();
    List<KitItem> wrongItems = new List<KitItem>();

    [Header("包包設定")]
    public Transform pack;  // 接收物品的BoxCaster
    public Transform packObj;
    public float castBoxRadius = .5f;

    [Header("UI設定")]
    public Transform checkedUiParent;
    public GameObject checkedUiObject;

    public OrbitQuickSetting defaultOrbitSetting;

    private void Start()
    {
        foreach (var item in items)
        {
            if (item.isCorrect)
                correctItems.Add(item);
            else
                wrongItems.Add(item);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pack.position, castBoxRadius * Vector3.one);
    }

    // exception是例外，必須抽出的物品
    public void ShowPack(bool b)
    {
        packObj.gameObject.SetActive(b);
    }

    void DrawItem(int correctCount, int wrongCount, params KitItem[] vipItems)
    {
        foreach (var item in items)
            item.gameObject.SetActive(false);

        var correctTemps = correctItems.Randomize<KitItem>();
        var wrongTemps = wrongItems.Randomize<KitItem>();

        var results = new List<Transform>();
        foreach (var vip in vipItems)
        {
            results.Add(vip.transform);
            if (vip.isCorrect)
                correctCount--;
            else
                wrongCount--;
        }


        for (int i = 0; i < correctCount; ++i)
            results.Add(correctTemps[i].transform);
        for (int i = 0; i < wrongCount; ++i)
            results.Add(wrongTemps[i].transform);

        results = results.Randomize<Transform>() as List<Transform>;

        defaultOrbitSetting.transforms = results.ToArray();

        foreach (var item in results)
            item.gameObject.SetActive(true);

        defaultOrbitSetting.Orbit();
    }

    [ContextMenu("Test Draw")]
    void TestDraw()
    {
        DrawItem(4, 2);
    }

    public void KitMissionSetup(int capacity, System.Action<int> onComplete, int correctCount, int wrongCount, params KitItem[] vipItems)
    {
        ShowPack(true);

        DrawItem(correctCount, wrongCount, vipItems);

        StartCoroutine(KitMission(capacity, onComplete));
    }

    IEnumerator KitMission(int capacity, System.Action<int> onComplete) // Action<int> 表示正確的數量
    {
        int inPackCount = 0;
        int correctCount = 0;
        while (capacity > inPackCount)
        {
            foreach (var hit in Physics.BoxCastAll(pack.position, Vector3.one * castBoxRadius, pack.forward))
            {
                if (hit.transform.GetComponent<KitItem>())
                {
                    var kit = hit.transform.GetComponent<KitItem>();
                    if (kit.IsGrabbing || !kit.hasTaken)
                        continue;
                    kit.inPack = true;
                    hit.transform.gameObject.SetActive(false);
                    inPackCount++;

                    if (kit.isCorrect)
                    {
                        correctCount++;
                        JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.wrong_v1);
                    }
                    else
                    {
                        JacDev.Audio.AudioHandler.Singleton.PlaySound(JacDev.Audio.AudioHandler.Singleton.soundList.wrong_v2);
                    }

                    print(inPackCount);
                }
            }

            yield return null;
        }

        onComplete.Invoke(correctCount);
    }
}
