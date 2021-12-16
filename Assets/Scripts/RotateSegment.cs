using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSegment : MonoBehaviour
{
    public Transform legTarget1;
    public Transform legTarget2;
    public Vector3 normalVec;
    void Awake()
    {
        legTarget1 = gameObject.transform.GetChild(0).GetComponent<stonLegMove>().targetTransform;
        legTarget2 = gameObject.transform.GetChild(1).GetComponent<stonLegMove>().targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToLeg();
    }
    void RotateToLeg()
    {
        normalVec = Vector3.Cross(transform.right, legTarget1.position-legTarget2.position).normalized;


        transform.rotation = Quaternion.FromToRotation(transform.up, normalVec) * transform.rotation;


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, normalVec.normalized);
    }
}
