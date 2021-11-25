using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMove : MonoBehaviour
{
    [Tooltip("Leg's IK target")]
    public Transform targetTransform;
    [Tooltip("GameObject(position) from wchich raycast(that determinates leg's position to move) starts")]
    public Transform rootTransform;
    public float minDistToStep = 0.05f;
    public float maxDistFromGround = 10f;
    public float maxDistToLeg = 1f;
    public float speed = 1f;
    [Tooltip("Better not to touch it")]
    public float journeyTime = 1.0f;
    [Tooltip("If you want leg move only when opposite leg is grounded, check it")]
    public bool moveWhenGrounded;
    [Tooltip("Oposite leg, assign if you have checked 'moveWhenGrounded'")]
    public LegMovement oppositeLegMovement1;
    public LegMovement oppositeLegMovement2;
    public bool isUp = false;
    public int smoothness = 1;
    private float startTime;
    public float stepHeight = 0.15f;
    Vector3 endPos;
    Vector3 startPos;

    void Awake()
    {
        RaycastHit hit;
        Physics.Raycast(rootTransform.position, rootTransform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        targetTransform.position = hit.point;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(rootTransform.position, rootTransform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        endPos = hit.point;
        if (moveWhenGrounded)
        {
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false && !oppositeLegMovement1.isUp && !oppositeLegMovement2.isUp)
            {
                isUp = true;
                startPos = targetTransform.position;
                startTime = Time.time;

            }
        }
        else
        {
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false)
            {
                isUp = true;
                startPos = targetTransform.position;
                startTime = Time.time;

            }
        }

        if (isUp == true)
        {
            StartCoroutine(PerformStep());   
        }
        
    }
    IEnumerator PerformStep()
    {
        for(int i = 1;i<= smoothness; i++)
        {
            targetTransform.position = Vector3.Lerp(startPos, endPos, i / (float)(smoothness + 1f));
            targetTransform.position += transform.up * Mathf.Sin(i / (float)(smoothness + 1f) * Mathf.PI) * stepHeight;
            yield return new WaitForFixedUpdate();
            Debug.Log("forek" + i);
        }
        Debug.Log("cykesn po forze");
        targetTransform.position = endPos;
        isUp = false;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Debug.DrawRay(rootTransform.position, rootTransform.TransformDirection(Vector3.down), Color.green);
    }








}
