using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData    // still in progress
{
    public string stuID;

    [System.Serializable]
    public class MissionData
    {
        public string name;
        public float time;
        public int score;
        public bool complete;

        public MissionData(string name, float time, int score, bool complete)
        {
            this.name = name;
            this.time = time;
            this.score = score;
            this.complete = complete;
        }
    }

    public List<MissionData> missionDatas;

    //Constructor
    public PlayerData()
    {
        stuID = DateTime.Now.ToString("MM-dd-yyyy");
        missionDatas = new List<MissionData>();
    }

    public void SetStageData(MissionData data)
    {

        if (missionDatas.Find((md) => md.name == data.name) != null)
        {
            int index = missionDatas.FindIndex((md) => md.name == data.name);
            missionDatas[index] = data;
            return;
        }

        missionDatas.Add(data);
    }

    public void SetName(string name)
    {
        stuID = name;
    }

    public MissionData GetMissionData(string name)
    {
        for (int i = 0; i < missionDatas.Count; ++i)
        {
            if (missionDatas[i].name == name)
                return missionDatas[i];
        }

        return null;
    }
}
