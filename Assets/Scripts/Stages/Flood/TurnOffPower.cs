using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPower : Stage
{
    public GameObject electronicBoxDoor;
    public GameObject interactArea;

    public override void OnBegin()
    {
        Transform t = electronicBoxDoor.transform;
        iTween.RotateAdd(electronicBoxDoor, new Vector3(0, -180, 0), 2f);

        if (spawnpoint != null)
            GameHandler.Singleton.SetLineGuider(true, spawnpoint.position);

        interactArea.gameObject.SetActive(true);
        
        JacDev.Audio.Flood a = (JacDev.Audio.Flood)GameHandler.Singleton.audioHandler;
        a.PlaySound(a.turnOffSwitch);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
