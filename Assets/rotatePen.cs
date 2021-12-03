using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePen : MonoBehaviour
{
    public float maxDistFromGround = 10f;
    public float quaternionSpeed = 1f;
    private Quaternion newRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        if(hit.transform!= null)
            RotatePerToHitNormal(hit.normal);
    }
    void RotatePerToHitNormal(Vector3 normalVector)
    {
        newRotation = Quaternion.FromToRotation(transform.up, normalVector) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, quaternionSpeed * Time.deltaTime);
    }
}
