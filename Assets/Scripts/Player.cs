using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public bool isStop = true;

    public float mouseSensity = 2.5f;
    float counter = 0;
    float stopTime = 3f;

    public float speed;
    bool gettingUp;
    Rigidbody rb;

    public Canvas canvas;

    GameObject target;
    public bool hasTarget;  //是否有目標物

    public bool canMove = true;
    public bool canRotate = true;
    public float moveDistance = 100f;
    public bool isTeleport = false;
    public Vector3 teleportTarget;

    float originHeight;

    public CurveLineRenderer curveLine;

    public Transform foot;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        curveLine.gameObject.SetActive(false);
        originHeight = transform.position.y;

        if (GetComponentInChildren<NavMeshAgent>() != null)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -transform.up, out hit);
            NavMeshAgent agent = GetComponentInChildren<NavMeshAgent>();
            agent.baseOffset = Vector3.Distance(transform.position, hit.point);
        }
    }

    // void Update()
    // {

    //     if (canMove)
    //     {
    //         if (InputHandler.Singleton.isTouch && !InputHandler.Singleton.isHolding)
    //         {
    //             Ray r = new Ray(transform.localPosition, GameHandler.Singleton.cam.transform.forward * moveDistance);
    //             Debug.DrawRay(transform.localPosition, GameHandler.Singleton.cam.transform.forward * moveDistance, Color.yellow, 1f);
    //             RaycastHit hit;
    //             LayerMask layer = LayerMask.GetMask("Roof");

    //             if (Physics.Raycast(r, out hit, moveDistance, ~layer))
    //             {
    //                 Debug.Log(hit.transform.name);

    //                 if (!curveLine.gameObject.activeInHierarchy)
    //                     curveLine.gameObject.SetActive(true);
    //                 curveLine.gameObject.SetActive(true);

    //                 curveLine.SetPoints(
    //                         transform.localPosition + (new Vector3(
    //                             -Mathf.Cos(Mathf.Deg2Rad * GameHandler.Singleton.cam.transform.localEulerAngles.y),
    //                             0,
    //                             Mathf.Sin(Mathf.Deg2Rad * GameHandler.Singleton.cam.transform.localEulerAngles.y))) * .2f,
    //                         hit.point,
    //                         3f,
    //                         .5f);

    //                 if (hit.transform.tag == "Floor")
    //                 {
    //                     isTeleport = true;
    //                     teleportTarget = Vector3.Scale(hit.point, new Vector3(1, 0, 1));

    //                     curveLine.SetColor(curveLine.activeColor);
    //                 }
    //                 else
    //                 {
    //                     isTeleport = false;
    //                     curveLine.SetColor(curveLine.inactiveColor);
    //                     // if (curveLine.gameObject.activeInHierarchy)
    //                     //     curveLine.gameObject.SetActive(false);
    //                 }
    //             }


    //         }
    //         else
    //         {

    //             curveLine.gameObject.SetActive(false);
    //         }

    //         if (InputHandler.Singleton.isClick)
    //         {
    //             //curveLine.gameObject.SetActive(false);

    //             if (isTeleport)
    //             {
    //                 Teleport(teleportTarget);
    //                 //transform.localPosition = teleportTarget + Vector3.up * 3;
    //                 isTeleport = false;
    //             }

    //             InputHandler.Singleton.isClick = false;
    //         }

    //         if (InputHandler.Singleton.isHolding)
    //         {
    //             isStop = false;
    //             counter = 0;

    //             transform.Translate(
    //                 new Vector3(
    //                     Mathf.Sin(Mathf.Deg2Rad * GameHandler.Singleton.cam.transform.localEulerAngles.y) * speed * Time.deltaTime,
    //                     0,
    //                     Mathf.Cos(Mathf.Deg2Rad * GameHandler.Singleton.cam.transform.localEulerAngles.y) * speed * Time.deltaTime));

    //             isTeleport = false;
    //             curveLine.gameObject.SetActive(false);
    //         }

    //         if (Input.GetMouseButtonUp(0))
    //         {
    //             curveLine.gameObject.SetActive(false);
    //         }

    //         counter += Time.deltaTime;


    //     }

    //     if (canRotate)
    //         Tracking();
        

    // }

    public void SetTarget(GameObject target)
    {
        hasTarget = true;
        this.target = target;
    }

    public void SetCanMove(bool b)
    {
        // if (rb == null)
        //     rb = GetComponent<Rigidbody>();

        canMove = b;
        //rb.isKinematic = !b;
    }

    public void Teleport(Vector3 point)
    {
        iTween.MoveTo(gameObject, Vector3.up * originHeight + point, .5f);
    }

    public void Tracking()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y += Input.GetAxis("Mouse X") * mouseSensity;
        rotation.x -= Input.GetAxis("Mouse Y") * mouseSensity;

        transform.localRotation = Quaternion.Euler(rotation);
    }
}
