using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoroutineUtility;

public class GameIntro : MonoBehaviour
{
    public float time = 3f;  // 時間
    public AudioSource introSE;

    private void Start()
    {
        var sceneLoader = FindObjectOfType<AsyncLoadingScript>();
        var timer = new Timer(
            time,
            () => sceneLoader.LoadScene("MissionSelect")
        );

        introSE.PlayDelayed(time / 2);
    }
}
