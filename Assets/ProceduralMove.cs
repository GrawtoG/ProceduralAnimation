using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMove : MonoBehaviour
{
    [Min(0)]public float walkSpeed = 1f;
    [Min(0)]public float rotSpeed = 1f;
    public Transform front;
    public Transform right;
    Vector3 frontVector;
    Vector3 rightVector;
    public Transform[] legsTargets;
    float average;
    public float bodyOffset = 0.5f;
    Vector3 normalVec;
    public int smoothness = 1;
    public float maxBodyGroundDist = 0.8f;
    RaycastHit hit;
    public GameObject testObject;
    void Awake()
    {
        
        walkSpeed *= 0.01f;
    }
    

    // Update is called once per frame
    void Update()
    {
        frontVector = (front.position - transform.position).normalized;
        rightVector = (right.position - transform.position).normalized;
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, rotSpeed, 0));
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + rotSpeed, transform.rotation.z);
            //normalVec = Quaternion.AngleAxis(rotSpeed, Vector3.left) * normalVec; 
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -rotSpeed, 0));
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - rotSpeed, transform.rotation.z);
           // normalVec = Quaternion.AngleAxis(-rotSpeed, Vector3.up) * normalVec;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += frontVector * walkSpeed;
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= frontVector * walkSpeed;
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += rightVector * walkSpeed;

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= rightVector * walkSpeed;

        }
        RotatePerToNormalVector();
        MoveHeight();
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(-Vector3.up * Time.deltaTime,Space.Self);
        }

    }
    void RotatePerToNormalVector()
    {
        Vector3 side1 = legsTargets[0].position - legsTargets[3].position;
        Vector3 side2 = legsTargets[1].position - legsTargets[2].position;
        normalVec =  -Vector3.Cross(side1, side2).normalized;
  
        
        transform.rotation = Quaternion.FromToRotation(transform.up, normalVec) * transform.rotation;
     
    }
    void MoveHeight()
    {
        
        float sum = 0;
        
        for (int i = 0; i <= legsTargets.Length-1; i++)
        {
            sum += legsTargets[i].transform.position.y;
        }
        average = sum / (legsTargets.Length);
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit,Mathf.Infinity);
        //Debug.Log(Vector3.Distance(hit.point, transform.position));
        //Debug.Log(transform.position.y-average);
        Vector3 localPosLeg = transform.position - normalVec * (transform.position.y - average);
        Debug.Log(Vector3.Distance(transform.position,localPosLeg));
        testObject.transform.position = localPosLeg;
        //transform.localPosition += new Vector3(0,bodyOffset - Vector3.Distance(transform.position, localPosLeg), 0);
        if(Vector3.Distance(transform.position, localPosLeg) > bodyOffset)
        {
            //transform.Translate(-Vector3.up * Time.deltaTime*0.5f, Space.Self);
        }
        else if(Vector3.Distance(transform.position, localPosLeg) < bodyOffset) 
        {
           // transform.Translate(Vector3.up * Time.deltaTime*0.5f, Space.Self);
        }
       
    }
   

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Debug.DrawRay(transform.position, legsTargets[0].position - transform.position);
        Gizmos.color = Color.green;
        Debug.DrawRay(transform.position, -normalVec*bodyOffset);
    }

}
