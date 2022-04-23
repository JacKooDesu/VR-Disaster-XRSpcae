using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResultUI : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject missionContent;

    [Header("UI綁定")]
    public Text missionHeader;
    public Text congratulation;
    public Text score;

    public Transform missionContentParent;
    public Text missionHint;
    public Image missionIcon;

    public void Setup(){

    }

    // Update is called once per frame
    void Update()
    {

    }
}
