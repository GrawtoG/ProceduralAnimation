using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonLegMove : MonoBehaviour
{
    [Tooltip("Leg's IK target")]
    public Transform targetTransform;
    public Vector3 targetStartPositionOffset;
    [Tooltip("GameObject(position) from wchich raycast(that determinates leg's position to move) starts")]
    public Transform rootTransform;
    public float minDistToStep = 0.05f;
    public float maxDistFromGround = 10f;
    public float maxDistToLeg = 1f;
    public float speed = 1f;


    [Tooltip("If you want leg move only when opposite leg is grounded, check it")]
    public bool moveWhenGrounded;
    [Tooltip("Oposite leg, assign if you have checked 'moveWhenGrounded'")]
    public stonLegMove oppositeLegMovement;

    public bool isUp = false;
    bool goUp = false;
    public float legUp = 1f;
   
    public bool canMove = false;
    Vector3 endPos;
    Vector3 startEndPos;
    Vector3 startPos;
    Vector3 halfPos;
    Vector3 startHalfPos;
    public GameObject targetPrefab;
   

    void Awake()
    {
        
        targetTransform = Instantiate(targetPrefab, new Vector3(0, 0, 0), Quaternion.identity).transform;

        RaycastHit hit;
        Physics.Raycast(rootTransform.position, rootTransform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        targetTransform.localPosition = hit.point+targetStartPositionOffset;
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
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false&&canMove)
            {
                canMove = false;
                startPos = targetTransform.position;
                startEndPos= endPos;
                startHalfPos = transform.position;
                isUp = true;
                goUp = true;

                
            }
        }
        else
        {
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false)
            {
                startPos = targetTransform.position;
                startEndPos = endPos; 
                startHalfPos = transform.position;
                isUp = true;
                goUp = true;


            }
        }

        if (isUp && goUp)
        {
            GetHalfPos();
            targetTransform.Translate((halfPos - targetTransform.position).normalized * Time.deltaTime * speed, Space.World);
            if (Vector3.Distance(targetTransform.position, halfPos) < minDistToStep)
            {
                goUp = false;
            }
        }
        if (isUp && !goUp)
        {
            GetHalfPos();
            targetTransform.Translate((endPos - targetTransform.position).normalized * Time.deltaTime * speed, Space.World);
            if (Vector3.Distance(targetTransform.position, endPos) < minDistToStep)
            {
                isUp = false;
                oppositeLegMovement.canMove = true;
            }
        }

    }
    void GetHalfPos()
    {

        halfPos = startPos + (endPos - startPos) * 0.5f;
        
        halfPos += new Vector3(0, legUp, 0);
        halfPos += (transform.position - startHalfPos);
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
