using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XRSpace.Platform;
using XRSpace.Platform.InputDevice;
using XRSpace.Platform.VRcore;
using UnityEngine.Serialization;

public class CustomControllerBehaviour : MonoBehaviour
{
    public XRCTLRaycaster CTLRaycaster;
    public XRHandlerDeviceType Device;
    public XRControllerButton TeleportButton;
    public XRControllerButton TriggerButton;
    public XRControllerButton GrabButton = XRControllerButton.Grab;

    public Transform BeamRender;
    // Line
    public LineRenderer DistanceRenderer;
    public LineRenderer InfinityRenderer;

    // Dot
    public Transform EndPoint;
    public Material DotMaterial;
    private Vector3 _currentHitPos;
    private bool _shouldUpdateDot = false;

    //teleport
    private XRRaycasterUtils.TeleportState _teleportState;
    private Vector3 _teleportPos;
    private Vector3 _tpRecenterDir;
    public static UnityAction<Quaternion> OnTeleportEnd;

    //touch pad
    public Transform TouchPadDot;
    private Vector2 _lastTouchPadPos = Vector2.zero;
    private bool _startTouch;

    // teleport point
    TeleportPoint selectingPoint;

    // object interacting
    bool isGrabbing;
    InteracableObject grabbingObj;
    float grabTime;

    Player player;

    private void Start()
    {
        if (EndPoint)
            EndPoint.GetComponent<MeshRenderer>().sortingOrder = 2;

        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        CTLRaycaster.TeleportEvent += UpdateTeleportStatus;
        CTLRaycaster.AfterRaycasterEvent += DrawBeam;
        CTLRaycaster.AfterRaycasterEvent += UpdateGrabState;
    }

    private void OnDisable()
    {
        CTLRaycaster.TeleportEvent -= UpdateTeleportStatus;
        CTLRaycaster.AfterRaycasterEvent -= DrawBeam;
        CTLRaycaster.AfterRaycasterEvent -= UpdateGrabState;
        HideAllLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (CTLRaycaster)
        {
            CTLRaycaster.HeadHeight = XRManager.Instance.transform.localPosition.y + XRManager.Instance.head.localPosition.y;
            if (!Application.isEditor || XRInputManager.Instance.EditorMode == XREditorMode.Simulator)
            {
                CTLRaycaster.Origin = transform.position;
                CTLRaycaster.Direction = transform.forward;
                CTLRaycaster.IsPress = XRInputManager.Instance.Button((XRDeviceType)Device, TriggerButton);
                CTLRaycaster.CanDrag = XRInputManager.Instance.Button((XRDeviceType)Device, TriggerButton);
                if (XRInputManager.Instance.Button((XRDeviceType)Device, XRControllerButton.TrackPadTouch) && !XRInputManager.Instance.Button((XRDeviceType)Device, XRControllerButton.TrackPadPress))
                {
                    if (!_startTouch)
                    {
                        //Debug.Log("[XRSDK] first _lastTouchPadPos");
                        _lastTouchPadPos = XRInputManager.Instance.TouchPosition((XRDeviceType)Device);
                    }
                    var delta = (XRInputManager.Instance.TouchPosition((XRDeviceType)Device) - _lastTouchPadPos);
                    var signX = Mathf.Sign(delta.x);
                    var signY = Mathf.Sign(delta.y);
                    delta *= 10;
                    delta = new Vector3(signX * Mathf.Round(Mathf.Abs(delta.x)), signY * Mathf.Floor(Mathf.Abs(delta.y)));
                    delta /= 10;
                    CTLRaycaster.ScrollDelta = delta;
                    //_lastTouchPadPos = XRInputManager.Instance.TouchPosition((XRDeviceType)Device);
                    _startTouch = true;
                }
                else
                    _startTouch = false;
            }
        }

        if (XRInputManager.Instance.ButtonUp((XRDeviceType)Device, TeleportButton)
            && _teleportState == XRRaycasterUtils.TeleportState.CanTeleport)
        {
            Teleport();
        }
        ProcessTouchpadDot();

        if (isGrabbing)
        {
            MoveObject();
            if (!grabbingObj.Interactable)
                Release();
        }


        //for editor mouse control
        if (Application.isEditor && XRInputManager.Instance.EditorMode != XREditorMode.Simulator)
        {
            if (XRInputManager.Instance.EditorMode == XREditorMode.Mouse)
            {
                if (Device == XRHandlerDeviceType.HANDLER_LEFT || Device == XRHandlerDeviceType.HANDLER_RIGHT)
                {
                    if (Input.GetMouseButtonDown(0)) //change to only mouseleft
                    {

                        if (CTLRaycaster != null)
                        {
                            CTLRaycaster.IsPress = true;
                            CTLRaycaster.CanDrag = true;
                            DotMaterial.SetFloat("_Blend", 1);
                        }

                        if (_teleportState == XRRaycasterUtils.TeleportState.CanTeleport)
                        {
                            Teleport();
                        }

                    }

                    if (Input.GetMouseButtonUp(0)) //change to only mouseleft
                    {

                        if (CTLRaycaster != null)
                        {
                            CTLRaycaster.IsPress = false;
                            CTLRaycaster.CanDrag = false;
                            DotMaterial.SetFloat("_Blend", 0);
                        }
                    }

                }
            }
        }
        BeamRender.position = transform.position;
        BeamRender.rotation = XRManager.Instance.head.rotation;
    }

    private void UpdateTeleportStatus(Vector3 Origin, Vector3 Target, XRRaycasterUtils.TeleportState teleportState)
    {
        selectingPoint = null;

        _teleportState = teleportState;
        if (_teleportState == XRRaycasterUtils.TeleportState.NotProcess)
            return;

        // Teleport point 自動導正
        var targetTemp = Target;
        if (CTLRaycaster.HitResult.gameObject && CTLRaycaster.HitResult.gameObject.GetComponent<TeleportPoint>())
        {
            var teleportPoint = CTLRaycaster.HitResult.gameObject.GetComponent<TeleportPoint>();
            teleportPoint.BeingSelect(CTLRaycaster);
            targetTemp = teleportPoint.transform.position;
            selectingPoint = teleportPoint;
        }

        //var heigh = XRManager.Instance.transform.position.y;
        _teleportPos = targetTemp;
        Vector3 direction = targetTemp - XRManager.Instance.head.position;
        _tpRecenterDir = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
    }

    private void Teleport()
    {
        if (!GameHandler.Singleton.player.canMove)
            return;
        if (_teleportState != XRRaycasterUtils.TeleportState.CanTeleport || !CTLRaycaster.UseRaycast)
            return;

        var playerRotate = Quaternion.FromToRotation(Vector3.ProjectOnPlane(XRManager.Instance.head.forward, Vector3.up), _tpRecenterDir);
        XRManager.Instance.transform.rotation *= playerRotate;
        var playerShift = _teleportPos - XRManager.Instance.head.position;
        //y shift is from player floor pos to teleportPos 
        playerShift.y = _teleportPos.y - XRManager.Instance.transform.parent.position.y;
        XRManager.Instance.transform.parent.position += playerShift;
        if (!Application.isEditor || XRInputManager.Instance.EditorMode == XREditorMode.Simulator)
            OnTeleportEnd(playerRotate);

        // teleport point event
        if (selectingPoint != null)
            selectingPoint.BeingTeleported();

        player.onTeleportEvent.Invoke();
    }

    #region Grab
    // 讓控制器也能做出抓取的動作
    void UpdateGrabState(Vector3 origin, Vector3 direction, XRCTLRaycaster.RaycasterType type, RaycastResult result)
    {
        var input = XRInputManager.Instance;
        if (isGrabbing)
        {
            if (input.ButtonUp((XRDeviceType)Device, GrabButton))
                Release();

            return;
        }

        if (type != XRCTLRaycaster.RaycasterType.ObjectRaycaster)
            return;

        if (!CTLRaycaster)
            return;

        if (!result.gameObject.GetComponent<InteracableObject>())
            return;

        var obj = result.gameObject.GetComponent<InteracableObject>();
        if (!obj.Interactable)
            return;

        if (input.Button((XRDeviceType)Device, GrabButton))
        {
            Grab(obj);
        }
    }

    void Grab(InteracableObject obj)
    {
        if (!obj.canGrab)
            return;

        isGrabbing = true;
        grabbingObj = obj;
        grabTime = Time.time;

        obj.Grabbed();

        if (obj.GetComponent<Rigidbody>() != null)
        {
            var _rigidbody = obj.GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
        }
    }

    void Release()
    {
        if (grabbingObj != null)
        {
            if (grabbingObj.GetComponent<Rigidbody>() != null)
            {
                var _rigidbody = grabbingObj.GetComponent<Rigidbody>();
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
                if (grabbingObj.transform.parent == transform)
                    grabbingObj.transform.parent = null;
                // _rigidbody.AddForce(_handVector * 300, ForceMode.Impulse);
            }

            grabbingObj.Released();
            isGrabbing = false;

            grabbingObj = null;
        }
    }

    //not every user grab untill gameobject on hand
    private void MoveObject()
    {
        var handPos = transform.position;
        float journeyLength = Vector3.Distance(grabbingObj.transform.position, handPos);
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - grabTime) * 2;//2 speed
        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;
        grabbingObj.transform.position = Vector3.Lerp(grabbingObj.transform.position, handPos, fractionOfJourney);
        if (Vector3.Distance(grabbingObj.transform.position, handPos) == 0)
            grabbingObj.transform.parent = transform;
    }

    #endregion

    private void LateUpdate()
    {
        //LineRender render line at lateUpdate.
        //If EndPoint update position at DrawBeam, will see the point and line not sync.
        if (_shouldUpdateDot && EndPoint)
            BeamRenderUtils.UpdateHitDot(EndPoint, _currentHitPos, XRManager.Instance.head.position);
    }

    private void DrawBeam(Vector3 origin, Vector3 direction, XRCTLRaycaster.RaycasterType type, RaycastResult result)
    {
        if (type == XRCTLRaycaster.RaycasterType.UIRaycaster || type == XRCTLRaycaster.RaycasterType.ObjectRaycaster)
        {
            _currentHitPos = result.worldPosition;
            if (DistanceRenderer)
            {
                BeamRenderUtils.UpdateLine(DistanceRenderer, origin, result.worldPosition);
                DistanceRenderer.enabled = true;
            }
            if (EndPoint)
            {
                _shouldUpdateDot = true;
                EndPoint.gameObject.SetActive(true);
            }
            if (InfinityRenderer)
                InfinityRenderer.enabled = false;
        }
        else if (type == XRCTLRaycaster.RaycasterType.None)
        {
            if (DistanceRenderer)
                DistanceRenderer.enabled = false;
            if (EndPoint)
            {
                _shouldUpdateDot = false;
                EndPoint.gameObject.SetActive(false);
            }
            if (InfinityRenderer)
            {
                InfinityRenderer.enabled = true;
                BeamRenderUtils.UpdateLine(InfinityRenderer, origin, origin + direction * 3f);
            }
        }
        else
        {
            if (DistanceRenderer)
                DistanceRenderer.enabled = false;
            if (EndPoint)
            {
                _shouldUpdateDot = false;
                EndPoint.gameObject.SetActive(false);
            }
            if (InfinityRenderer)
            {
                InfinityRenderer.enabled = false;
            }
        }
    }

    private void ProcessTouchpadDot()
    {
        if (TouchPadDot)
        {
            TouchPadDot.gameObject.SetActive(XRInputManager.Instance.Button((XRDeviceType)Device, XRControllerButton.TrackPadTouch));
            TouchPadDot.localPosition = 15 * XRInputManager.Instance.TouchPosition((XRDeviceType)Device);
        }
    }

    private void HideAllLine()
    {
        _shouldUpdateDot = false;
        if (DistanceRenderer)
            DistanceRenderer.enabled = false;
        if (EndPoint)
            EndPoint.gameObject.SetActive(false);
        if (InfinityRenderer)
            InfinityRenderer.enabled = false;
    }
}
