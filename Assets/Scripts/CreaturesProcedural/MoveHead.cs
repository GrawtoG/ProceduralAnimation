using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHead : MonoBehaviour
{
    public bool moveHeight = true;
    public bool rotatePer = true;
    public float maxDistFromGround = 10f;
    public float quaternionSpeed = 1f;
    private Quaternion newRotation;
    public Vector3 rayOffset;
    public float headFromGroundOffset;
    RaycastHit hit;
    public float smoothness = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveHeight || rotatePer)
        {
            Physics.Raycast(transform.position + rayOffset, transform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
        }
        if (rotatePer)
            RotatePerToHitNormal(hit.normal);
        if (moveHeight)
            MoveHeight();
        
    }
    void MoveHeight()
    {
        transform.position += (1 / smoothness) * (transform.up * Time.fixedDeltaTime * (headFromGroundOffset - Vector3.Distance(transform.position, hit.point)));

    }
    void RotatePerToHitNormal(Vector3 normalVector)
    {
        newRotation = Quaternion.FromToRotation(transform.up, normalVector) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, quaternionSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + rayOffset, transform.TransformDirection(Vector3.down));
    }
}
