using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Transform head;

    public bool isStop = true;

    float counter = 0;
    float stopTime = 3f;

    Rigidbody rb;

    public HintCanvas hintCanvas;

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

        hintCanvas.head = head;

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
}
