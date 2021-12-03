using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonMove : MonoBehaviour
{
    public Transform frontTransform;
    Vector3 frontVector;
    public float walkSpeed = 1f;
    public float rotSpeed = 1f;
    void Awake()
    {
       // frontTransform.position = new Vector3(frontTransform.position.x, transform.position.y, frontTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //moveDirection += frontVector;
        frontVector = (frontTransform.position - transform.position).normalized;
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += frontVector * walkSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= frontVector * walkSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, rotSpeed*Time.deltaTime*50, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -rotSpeed*Time.deltaTime*50, 0));
        }
    }
}
