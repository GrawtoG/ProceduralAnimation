using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonLegMove : MonoBehaviour
{
    [Tooltip("Leg's IK target")]
    public Transform targetTransform;
    [Tooltip("GameObject(position) from wchich raycast(that determinates leg's position to move) starts")]
    public Transform rootTransform;
    public float minDistToStep = 0.05f;
    public float maxDistFromGround = 10f;
    public float maxDistToLeg = 1f;
    public float speed = 1f;


    [Tooltip("If you want leg move only when opposite leg is grounded, check it")]
    public bool moveWhenGrounded;
    [Tooltip("Oposite leg, assign if you have checked 'moveWhenGrounded'")]
    public LegMove[] oppositeLegMovement;

    public bool isUp = false;
    bool goUp = false;
    public float legUp = 1f;
    private bool oppositeLegsUp;

    Vector3 endPos;
    Vector3 startPos;
    Vector3 halfPos;
    public GameObject targetPrefab;

    void Awake()
    {
        
        targetTransform = Instantiate(targetPrefab, new Vector3(0, 0, 0), Quaternion.identity).transform;

        RaycastHit hit;
        Physics.Raycast(rootTransform.position, rootTransform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        targetTransform.position = hit.point;
        isUp = false;
        goUp = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     
        RaycastHit hit;
        Physics.Raycast(rootTransform.position, rootTransform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        endPos = hit.point;
        if (hit.collider == null)
        {

            targetTransform.position = transform.parent.position;
        }
        if (moveWhenGrounded)
        {
            for (int i = 0; i < oppositeLegMovement.Length; i++)
            {
                if (oppositeLegMovement[i].isUp)
                {
                    oppositeLegsUp = false;
                }
            }
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false && !oppositeLegsUp)
            {
                oppositeLegsUp = true;
                startPos = targetTransform.position;
                GetHalfPos();
                isUp = true;
                goUp = true;

                
            }
        }
        else
        {
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false)
            {
                startPos = targetTransform.position;
                GetHalfPos();
                isUp = true;
                goUp = true;


            }
        }

        if (isUp && goUp)
        {
            targetTransform.Translate((halfPos - targetTransform.position).normalized * Time.deltaTime * speed, Space.World);
            if (Vector3.Distance(targetTransform.position, halfPos) < minDistToStep)
            {
                goUp = false;
            }
        }
        if (isUp && !goUp)
        {
            targetTransform.Translate((endPos - targetTransform.position).normalized * Time.deltaTime * speed, Space.World);
            if (Vector3.Distance(targetTransform.position, endPos) < minDistToStep)
            {
                isUp = false;
            }
        }

    }
    void GetHalfPos()
    {
        halfPos = startPos + (endPos - startPos) * 0.5f;
        halfPos += new Vector3(0, legUp, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(halfPos, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPos, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetTransform.position, 0.2f);
        //Debug.DrawRay(rootTransform.position, rootTransform.TransformDirection(Vector3.down));
    }

}
