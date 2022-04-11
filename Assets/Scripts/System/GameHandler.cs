using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using JacDev.Audio;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class GameHandler : MonoBehaviour
{
    static GameHandler singleton = null;

    public static GameHandler Singleton
    {
        get
        {
            singleton = FindObjectOfType(typeof(GameHandler)) as GameHandler;

            if (singleton == null)
            {
                GameObject g = new GameObject("GameHandler");
                singleton = g.AddComponent<GameHandler>();
            }

            return singleton;
        }
    }

    [Header("Player")]
    public Player player;   // 玩家

    public PlayerData playerData;   // 玩家資料序列化

    [Header("Stage")]
    public Stage firstStage;    // 首 Stage
    // public Stage finalStage;    // 末 Stage

    [Header("Cameras")]
    public Camera cam;  // 相機
    public Camera uiCamera; // 專門渲染UI的相機

    public AudioHandler audioHandler;
    //public GameObject curveLineRendererPrefab;  // 曲線渲染預製物

    [SerializeField] Stage currentStage; // 當前 Stage

    public Transform ObjectParent;

    public AsyncLoadingScript sceneLoader;

    float timer = 0f;
    int score;

    private void Start()
    {
        // // 讀存檔系統，於Cardboard版本內無效
        // if (SceneLoader.Singleton.GetCurrentSceneName() != "Tutorial")
        // {
        //     if (LoadPlayerData(SceneLoader.Singleton.GetName()) != null)
        //     {
        //         SetPlayerName(System.DateTime.Now.ToString("MM-dd-yyyy"));
        //         SetPlayerData(LoadPlayerData(SceneLoader.Singleton.GetName()));
        //     }
        //     else
        //     {
        //         playerData = new PlayerData();
        //     }
        // }
        // else
        // {
        //     playerData = new PlayerData();
        // }

        if (player == null)
            player = FindObjectOfType<Player>();

        StartCoroutine(PlayStage(firstStage));
    }

    public IEnumerator PlayStage(Stage stg)
    {
        print(stg.name);
        currentStage = stg;

        if (stg.target != null)
            player.SetTarget(stg.target);

        stg.OnBegin();

        while (!stg.isFinish)
        {

            timer += Time.deltaTime;

            stg.OnUpdate();
            // UpdateLine();
            yield return null;
        }

        stg.OnFinish();

        StopCoroutine("Counter");

        if (stg.nextStage != null)
        {
            audioHandler.StopCurrent();
            stg.StopAllCoroutines();
            yield return StartCoroutine(PlayStage(stg.nextStage));
        }
        else
        {
            while (audioHandler.currentPlayingSound != null)
            {
                yield return null;
            }

            if (SceneLoader.Singleton.GetCurrentSceneName() != "Tutorial")
            {
                playerData.SetStageData(
                    new PlayerData.MissionData(SceneLoader.Singleton.GetCurrentSceneName(), timer, score, true));
            }
            // SavePlayerData();
            score = 0;
            sceneLoader.LoadScene("Tutorial");
        }

    }

    // 當前 Stage完成
    public void StageFinish()
    {
        currentStage.isFinish = true;
    }

    // 綁Event在物件上
    public void BindEvent(GameObject g, EventTriggerType type, UnityAction<BaseEventData> call)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(call);

        EventTrigger tempTrigger;
        if (g.GetComponent<EventTrigger>())
            tempTrigger = g.GetComponent<EventTrigger>();
        else
            tempTrigger = g.AddComponent<EventTrigger>();

        tempTrigger.triggers.Add(entry);
    }

    // 開關相機 Tracking
    public void SetVRCameraTracking(bool b)
    {
        UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(GameHandler.Singleton.cam, b);
    }


    public void BlurCamera(bool status)
    {
        BlurOptimized blur;
        if (cam.GetComponent<BlurOptimized>())
            blur = cam.GetComponent<BlurOptimized>();
        else
            blur = cam.gameObject.AddComponent<BlurOptimized>();

        blur.enabled = status;
    }
    /*
        public void GrayCamera(bool status)
        {
            Grayscale grayscale;
            if (cam.GetComponent<Grayscale>())
                grayscale = cam.GetComponent<Grayscale>();
            else
                grayscale = cam.gameObject.AddComponent<Grayscale>();

            grayscale.enabled = status;
        }
        */

    public IEnumerator Counter(float min, float max, IEnumerator nextFunction)
    {
        float t = Random.Range(min, max);
        float counter = 0;

        while (counter < t)
        {
            counter += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(nextFunction);
    }

    public IEnumerator Counter(float min, float max, UnityAction action)
    {
        float t = Random.Range(min, max);
        float counter = 0;

        while (counter < t)
        {
            counter += Time.deltaTime;

            yield return null;
        }

        action.Invoke();
    }

    public IEnumerator Counter(float t, UnityAction action)
    {
        float counter = 0;
        while (counter < t)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        action.Invoke();
    }

    public void MovePlayer(Transform t)
    {
        player.transform.position = t.position;
        player.transform.rotation = t.rotation;
    }

    public void SavePlayerData()
    {
        FileManager<PlayerData>.Save($"{playerData.stuID}_Data", playerData, "PlayerData");
        print("Save");
    }

    public void LoadPlayerData(string name)
    {
        FileManager<PlayerData>.Load("PlayerData", $"{name}_Data", playerData);
    }

    public void SetPlayerData(PlayerData d)
    {
        playerData = d;
    }

    public void SetPlayerName(Text text)
    {
        playerData.stuID = text.text;
    }

    public void SetPlayerName(string text)
    {
        playerData.stuID = text;
        SceneLoader.Singleton.SetName(text);
    }

    public Stage GetCurrentStage()
    {
        return currentStage;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
