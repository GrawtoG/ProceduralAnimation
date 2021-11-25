using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
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

    private float startTime;

    Vector3 endPos;
    Vector3 startPos;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;
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
            if (Vector3.Distance(targetTransform.position, endPos) > maxDistToLeg && isUp == false &&!oppositeLegMovement1.isUp&& !oppositeLegMovement2.isUp)
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
        
        if(isUp == true)
        {
            GetCenter(Vector3.up);
            float fracComplete = Mathf.PingPong(Time.time - startTime, journeyTime / speed);
            targetTransform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            targetTransform.position += centerPoint;
            if (fracComplete >= 1)
            {
                startTime = Time.time;
            }
        }
        if (Vector3.Distance(targetTransform.position, endPos) < minDistToStep)
        {
            isUp = false;
        }
    }
    void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos + endPos);
        centerPoint -= direction;
        startRelCenter = startPos - centerPoint;
        endRelCenter = endPos - centerPoint;


    }
    


        
      
     
    

    
}
