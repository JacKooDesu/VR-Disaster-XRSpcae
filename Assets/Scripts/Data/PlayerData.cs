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
        public bool complete;
    }

    public List<MissionData> missionDatas;

    //Constructor
    public PlayerData()
    {
        stuID = DateTime.Now.ToString("MM-dd-yyyy");
        missionDatas = new List<MissionData>();
    }

    public void SetStageData(string missionName, float time, bool isComplete)
    {
        MissionData data = new MissionData();
        data.name = missionName;
        data.time = time;
        data.complete = isComplete;

        for (int i = 0; i < missionDatas.Count; ++i)
        {
            if (missionDatas[i].name == missionName)
            {
                missionDatas[i] = data;
                return;
            }
        }

        missionDatas.Add(data);
    }

    public void SetName(string name){
        stuID = name;
    }

    public MissionData GetMissionData(string name){
        for(int i = 0; i < missionDatas.Count; ++i){
            if(missionDatas[i].name == name)
                return missionDatas[i];
        }

        return null;
    }
}
