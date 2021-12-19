using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segmentMove : MonoBehaviour
{
    // Start is called before the first frame update



    public bool ok = false;
    float averageY;
    float averageZ;
    float averageX;

    public Transform legTarget1;
    public Transform legTarget2;

    public Transform previousSegment;

    public Vector3 normalVec = new Vector3(0,0,0);

    public float smoothness = 1;

    public float segmentOffset = 1;

    void Awake()
    {

        StartCoroutine(GetLegTarget());
        StartCoroutine(GetPreviousSegment());

    }


    // Update is called once per frame
    void Update()
    {
        if (legTarget1 != null)
        {

            RotateToLeg();
            MoveHeight();
        }
        if (previousSegment != null)
        {
            //transform.LookAt(previousSegment.position);
            //transform.forward = (previousSegment.position - transform.position).normalized;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, previousSegment.rotation, 10 * Time.deltaTime * 50);
        }


    }
    void RotateToLeg()
    {

        normalVec = Vector3.Cross(transform.right, legTarget1.position - legTarget2.position).normalized;


       // transform.up = transform.TransformDirection(normalVec);
        transform.rotation = Quaternion.FromToRotation(transform.up, normalVec) * transform.rotation;
    }



    void MoveHeight()
    {
        Vector3 averageLegPos;
        float sumY;
        float sumX;
        float sumZ;



        sumY = legTarget1.transform.position.y + legTarget2.transform.position.y;
        sumX = legTarget1.transform.position.x + legTarget2.transform.position.x;
        sumZ = legTarget1.transform.position.z + legTarget2.transform.position.z;

        averageY = sumY / 2;
        averageX = sumX / 2;
        averageZ = sumZ / 2;
        averageLegPos = new Vector3(averageX, averageY, averageZ);

        transform.position += (1 / smoothness) * (transform.up * Time.fixedDeltaTime * (segmentOffset - Vector3.Distance(transform.position, averageLegPos)));
    }

    IEnumerator GetLegTarget()
    {

        yield return new WaitUntil(() => gameObject.transform.GetChild(0).GetComponent<stonLegMove>().targetTransform != null);
        legTarget1 = gameObject.transform.GetChild(0).GetComponent<stonLegMove>().targetTransform;
        legTarget2 = gameObject.transform.GetChild(1).GetComponent<stonLegMove>().targetTransform;
    }
    IEnumerator GetPreviousSegment()
    {
        yield return new WaitForSeconds(0.1f);
        Transform parent = transform.parent;
        for (int i = 0; i < transform.parent.parent.childCount; i++)
        {
            if (parent.parent.GetChild(i).GetChild(0).transform == transform)
            {
                if (i == 1)
                {
                    previousSegment = parent.parent.GetChild(i - 1).transform;
                }
                else
                {
                    previousSegment = parent.parent.GetChild(i - 1).GetChild(0).transform;
                }

            }
        }
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(transform.position, normalVec);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(normalVec));
    }
}
