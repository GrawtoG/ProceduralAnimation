using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMove : MonoBehaviour
{
    [Min(0)] public float walkSpeed = 1f;
    [Min(0)] public float rotSpeed = 1f;
    public Transform front;
    public Transform right;
    Vector3 frontVector;
    Vector3 rightVector;
    public Transform[] legsTargets;
    float averageY;
    float averageZ;
    float averageX;
    Vector3 side1;
    Vector3 side2;
    public float bodyOffset = 0.5f;
    public float maxDistFromGround = 10;
    public float quaternionSpeed = 1;
    Vector3 normalVec;
    public float smoothness = 1;
    public float maxBodyGroundDist = 0.8f;
    RaycastHit hit;
    public GameObject testObject;
    public GameObject[] legRoots;
    public bool rotatingBodyLegs = false;
    public bool rotatingBodyHitNormal = true;
    public bool upDownBody = true;
    public bool tiltRoots = false;
    public bool isUpReallyUp = true;
    public float angle = 30f;
    Quaternion newRotation;
    Vector3 moveDirection;
    Vector3 previousPos;
    Vector3 Axis;
    void Awake()
    {
        legRoots = GameObject.FindGameObjectsWithTag("LegRoot");
        front.position = new Vector3(front.position.x, transform.position.y, front.position.z);
        right.position = new Vector3(right.position.x, transform.position.y, right.position.z);



    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (upDownBody)
        {
            MoveHeight();
        }
        if (rotatingBodyLegs)
        {
            RotatePerToNormalVectorLegs();
        }
        if (rotatingBodyHitNormal)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, maxDistFromGround);
            RotatePerToHitNormal(hit.normal);
        }

        frontVector = (front.position - transform.position).normalized;
        rightVector = (right.position - transform.position).normalized;
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, rotSpeed*Time.deltaTime*50, 0));
            moveDirection += rightVector;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -rotSpeed*Time.deltaTime*50, 0));
            moveDirection -= rightVector;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += frontVector * walkSpeed * Time.fixedDeltaTime;
            moveDirection += frontVector;


        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= frontVector * walkSpeed * Time.fixedDeltaTime;
            moveDirection -= frontVector;

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += rightVector * walkSpeed * Time.fixedDeltaTime;
            moveDirection += rightVector;

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= rightVector * walkSpeed * Time.fixedDeltaTime;
            moveDirection -= rightVector;

        }
        moveDirection.Normalize();
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) && tiltRoots)
        {

            Debug.Log("gora");
            TiltRoots(-angle);

        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && tiltRoots)
        {
            Debug.Log("dol");
            TiltRoots(angle);
        }

        void TiltRoots(float _angle)
        {
            Axis = Vector3.Cross(moveDirection.normalized, transform.up);
            foreach (GameObject root in legRoots)
            {

                root.transform.Rotate(Axis, _angle);

                //root.transform.localEulerAngles = ;
            }
        }
        void RotatePerToNormalVectorLegs()
        {

            if (legsTargets.Length % 2 == 0 && legsTargets.Length > 1)
            {
                side1 = legsTargets[0].position - legsTargets[3].position;
                side2 = legsTargets[1].position - legsTargets[2].position;
            }
            if (legsTargets.Length % 2 != 0 && legsTargets.Length > 1)
            {
                side1 = legsTargets[1].position - legsTargets[0].position;
                side2 = legsTargets[2].position - legsTargets[0].position;
            }

            normalVec = -Vector3.Cross(side1, side2).normalized;


            transform.rotation = Quaternion.FromToRotation(transform.up, normalVec) * transform.rotation;
        }
        void RotatePerToHitNormal(Vector3 normalVector)
        {
            newRotation = Quaternion.FromToRotation(transform.up, normalVector) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, quaternionSpeed * Time.deltaTime);
        }
        void MoveHeight()
        {
            Vector3 averageLegPos;
            float sumY = 0;
            float sumX = 0;
            float sumZ = 0;

            for (int i = 0; i <= legsTargets.Length - 1; i++)
            {
                sumY += legsTargets[i].transform.position.y;
                sumX += legsTargets[i].transform.position.x;
                sumZ += legsTargets[i].transform.position.z;
            }
            averageY = sumY / (legsTargets.Length);
            averageX = sumX / (legsTargets.Length);
            averageZ = sumZ / (legsTargets.Length);
            averageLegPos = new Vector3(averageX, averageY, averageZ);
            if (isUpReallyUp)
            {
                transform.position += (1 / smoothness) * (transform.up * Time.fixedDeltaTime * (bodyOffset - Vector3.Distance(transform.position, averageLegPos)));
            }


        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, moveDirection.normalized);
    }
}
