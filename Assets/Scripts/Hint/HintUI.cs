using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HintUI : MonoBehaviour
{
    HintObejctCamera hintObejctCamera;
    public HintObject traceObject;
    CanvasGroup canvasGroup;

    public Text objectName;
    public Image image;

    bool state = false;
    bool stateLast = false;
    [SerializeField] int tweenId;

    // public void Setup(HintObject ho, HintObejctCamera hCamera)
    // {
    //     hintObejctCamera = hCamera;
    //     this.traceObject = ho;
    //     objectName.text = ho.objectName;
    //     image.sprite = ho.image;
    //     canvasGroup = GetComponent<CanvasGroup>();

    //     tweenId = transform.GetInstanceID();
    // }

    private void Start()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (traceObject.hasRenderTime >= traceObject.limitTime)
        {
            state = true;
        }
        else
        {
            state = false;
        }

        if (state != stateLast)
        {
            if (state == true)
                DoFade(1f);
            if (state == false)
                DoFade(0f);

            stateLast = state;
        }
    }

    private void LateUpdate()
    {
        // transform.localPosition = Util.UIMath.WorldToCanvasPosition(
        //                     traceObject.transform, hintObejctCamera.cam, hintObejctCamera.canvas);
        var head = GameHandler.Singleton.player.head;
        transform.LookAt(transform.position + head.rotation * Vector3.forward, head.rotation * Vector3.up);
    }

    void DoFade(float value)
    {
        if (DOTween.TweensById(tweenId) != null)
        {
            if (DOTween.TweensById(tweenId).Count != 0)
                DOTween.Kill(tweenId);
        }

        DOTween.To(() => canvasGroup.alpha, (f) => canvasGroup.alpha = f, value, .5f).SetId(tweenId);
    }
}