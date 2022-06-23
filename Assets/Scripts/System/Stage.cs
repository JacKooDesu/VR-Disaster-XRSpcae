using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stage : MonoBehaviour
{
    public Transform spawnpoint;  // 重生點，可不用

    // 顯示物件
    public StageObject[] stageObjects;

    [System.Serializable]
    public class StageObject
    {
        public GameObject obj;
        public int layer;
        public bool destroyOnFinish;
    }

    public GameObject target;  // 目的地
    public System.Action onGetToTarget; // 如果有目的地，到達時觸發

    public bool isFinish = false;   // 是否結束
    public Stage nextStage;     // 下一個Stage

    [Header("其他")]
    public int score;

    // Stage開始時
    public virtual void OnBegin()
    {
        // Stage物件顯示
        foreach (StageObject so in stageObjects)
        {
            if (so.layer != -1)
                so.obj.layer = so.layer;

            so.obj.SetActive(true);
        }

        if (target != null)
            GameHandler.Singleton.player.PathFinding(target.transform.position);
        //iTween.MoveTo(GameHandler.Singleton.player.gameObject, spawnpoint, .5f);
    }

    public virtual void OnUpdate()
    {
        if (target == null) return;

        if (Vector3.Distance(target.transform.position, GameHandler.Singleton.player.transform.position) <= 1f)
        {
            GameHandler.Singleton.player.line.gameObject.SetActive(false);
            if (onGetToTarget != null)
                onGetToTarget.Invoke();
            target = null;
        }
    }

    // Stage結束時
    public virtual void OnFinish()
    {
        // 隱藏所有Stage 物件
        foreach (StageObject so in stageObjects)
            if (so.obj != null) so.obj.SetActive(!so.destroyOnFinish);

        GameHandler.Singleton.player.line.gameObject.SetActive(false);
    }

    public T FindStageObject<T>()
    {
        foreach (var g in stageObjects)
        {
            if (g.obj.GetComponent<T>() != null)
                return g.obj.GetComponent<T>();
        }

        Debug.LogWarning("OBJECT MISSING!");
        return default(T);
    }

    public List<T> FindStageObjects<T>()
    {
        List<T> objects = new List<T>();

        foreach (var g in stageObjects)
        {
            if (g.obj.GetComponent<T>() != null)
                objects.Add(g.obj.GetComponent<T>());
        }

        return objects;
    }

    public List<T> FindStageObjects<T>(string nameContain)
    {
        List<T> objects = new List<T>();

        foreach (var g in stageObjects)
        {
            if (g.obj.GetComponent<T>() != null)
            {
                if (g.obj.name.Contains(nameContain))
                    objects.Add(g.obj.GetComponent<T>());
            }
        }

        return objects;
    }

    public void SubScore(int value)
    {
        if (score >= 0)
            score -= value;
    }

    public void SetFinish(bool b)
    {
        isFinish = b;
    }
}
